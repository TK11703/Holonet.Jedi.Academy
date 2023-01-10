using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Holonet.Jedi.Academy.App.Middleware;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Holonet.Jedi.Academy.App.Controllers
{
	[Route("api/quests")]
	[Produces("application/json")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	[ApiController]
	public class QuestsController : ControllerDefaults
	{
		private readonly ILogger<QuestsController> _logger;
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;
		private readonly IRazorPartialToStringRenderer _renderer;

		public QuestsController(ILogger<QuestsController> logger, Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IRazorPartialToStringRenderer renderer, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_logger = logger;
			_context = context;
			_userManager = userManager;
			_renderer = renderer;
		}

		[Route("GetIndexPage")]
		[HttpPost]
		public async Task<IActionResult> GetIndexPage([FromBody] JObject data)
		{
			if (data == null || data["textFilter"] == null || data["rankFilter"] == null || data["firstItem"] == null || data["completed"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				string textFilter = data["textFilter"].ToObject<string>();
				int? rankFilter = data["rankFilter"].ToObject<int?>();
				bool? completed = data["completed"].ToObject<bool?>();
				int firstItem = data["firstItem"].ToObject<int>();
				UserAccount? currentUser = GetActiveUser();
				if (currentUser == null)
				{
					throw new Exception("A valid user account was not detected.");
				}

				bool canCreateEdit = await CanCreateEditItem();

				IQueryable<Quest>? questsIQ = null;
				if (canCreateEdit)
				{
					questsIQ = from q in _context.Quests.Include(q => q.Rank)
							   from qp in _context.QuestParticipation.Where(x=>x.QuestId== q.Id && x.StudentId.Equals(GetStudentId(currentUser)) && x.CompletedOn.HasValue).DefaultIfEmpty()
							   select new Quest()
							   {
								   Id = q.Id,
								   Name = q.Name,
								   ExperienceToGain = q.ExperienceToGain,
								   Description = q.Description,
								   Rank = q.Rank,
								   RankId = q.RankId,
								   Archived = q.Archived,
								   CreatedOn = q.CreatedOn,
								   HasUserCompleted = qp.CompletedOn.HasValue,
								   CanUserCreateEdit= canCreateEdit
							   };
				}
				else
				{
					questsIQ = from q in _context.Quests.Include(q => q.Rank).Where(q => !q.Archived)
							   from qp in _context.QuestParticipation.Where(x => x.QuestId == q.Id && x.StudentId.Equals(GetStudentId(currentUser)) && x.CompletedOn.HasValue).DefaultIfEmpty()
							   select new Quest()
							   {
								   Id = q.Id,
								   Name = q.Name,
								   ExperienceToGain = q.ExperienceToGain,
								   Description = q.Description,
								   Rank = q.Rank,
								   RankId = q.RankId,
								   Archived = q.Archived,
								   CreatedOn = q.CreatedOn,
								   HasUserCompleted = qp.CompletedOn.HasValue,
								   CanUserCreateEdit = canCreateEdit
							   };
				}
				if (!String.IsNullOrEmpty(textFilter))
				{
					questsIQ = questsIQ.Where(x => x.Name.ToUpper().Contains(textFilter.ToUpper()) || (x.Description != null && x.Description.ToUpper().Contains(textFilter.ToUpper())));
				}

				if (rankFilter != null)
				{
					questsIQ = questsIQ.Where(x => x.RankId.Equals(rankFilter));
				}
				if(completed.HasValue)
				{
					questsIQ = questsIQ.Where(x => x.HasUserCompleted.Equals(completed));
				}
				questsIQ = questsIQ.OrderByDescending(s => s.CreatedOn);
				var pageSize = (Config.SiteSettings != null) ? Config.SiteSettings.PageSize : 10;
				// Extract a portion of data
				var pageData = await questsIQ.Skip(firstItem).Take(pageSize).ToListAsync();
				if (pageData == null || pageData.Count() == 0)
					return StatusCode(204);  // 204 := "No Content"
				var content = await _renderer.RenderPartialToStringAsync("~/Pages/Quests/_Index.cshtml", pageData);
				return Ok(content);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		private int GetStudentId(UserAccount currentUser)
		{
			int studentId = -1;
			var userProfileDomain = _context.UserProfiles.Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefault();
			if (userProfileDomain != null)
			{
				studentId = userProfileDomain.StudentId;
			}
			return studentId;
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
