using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Holonet.Jedi.Academy.App.Middleware;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Holonet.Jedi.Academy.App.Controllers
{
	[Route("api/experience")]
	[Produces("application/json")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	[ApiController]
	public class ExperienceController : ControllerDefaults
	{
		private readonly ILogger<ExperienceController> _logger;
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;
		private readonly IRazorPartialToStringRenderer _renderer;

		public ExperienceController(ILogger<ExperienceController> logger, Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IRazorPartialToStringRenderer renderer, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
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
			if (data == null || data["textFilter"] == null || data["firstItem"] == null || data["completed"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				string textFilter = data["textFilter"].ToObject<string>();
				bool? completed = data["completed"].ToObject<bool?>();
				int firstItem = data["firstItem"].ToObject<int>();
				UserAccount? currentUser = GetActiveUser();
				if (currentUser == null)
				{
					throw new Exception("A valid user account was not detected.");
				}

				bool canCreateEdit = await CanCreateEditItem();

				IQueryable<Knowledge>? knowledgeOpsIQ = null;
				if (canCreateEdit)
				{
					knowledgeOpsIQ = from ko in _context.KnowledgeOpportunities
									 from kl in _context.KnowledgeLearned.Where(x => x.KnowledgeId == ko.Id && x.StudentId.Equals(GetStudentId(currentUser)) && x.CompletedOn.HasValue).DefaultIfEmpty()
									 select new Knowledge()
									 {
										 Id = ko.Id,
										 Name = ko.Name,
										 ExperienceToGain = ko.ExperienceToGain,
										 Description = ko.Description,
										 Archived = ko.Archived,
										 CreatedOn = ko.CreatedOn,
										 HasUserCompleted = kl.CompletedOn.HasValue,
										 CanUserCreateEdit = canCreateEdit
									 };
				}
				else
				{
					knowledgeOpsIQ = from ko in _context.KnowledgeOpportunities.Where(ko => !ko.Archived)
									 from kl in _context.KnowledgeLearned.Where(x=>x.KnowledgeId==ko.Id && x.StudentId.Equals(GetStudentId(currentUser)) && x.CompletedOn.HasValue).DefaultIfEmpty()
									 select new Knowledge()
									 {
										 Id = ko.Id,
										 Name = ko.Name,
										 ExperienceToGain = ko.ExperienceToGain,
										 Description = ko.Description,
										 Archived = ko.Archived,
										 CreatedOn = ko.CreatedOn,
										 HasUserCompleted = kl.CompletedOn.HasValue,
										 CanUserCreateEdit = canCreateEdit
									 };
				}
				if (!String.IsNullOrEmpty(textFilter))
				{
					knowledgeOpsIQ = knowledgeOpsIQ.Where(x => x.Name.ToUpper().Contains(textFilter.ToUpper()));
				}

				if (completed.HasValue)
				{
					knowledgeOpsIQ = knowledgeOpsIQ.Where(x => x.HasUserCompleted.Equals(completed));
				}

				knowledgeOpsIQ = knowledgeOpsIQ.OrderByDescending(s => s.CreatedOn);
				var pageSize = (Config.SiteSettings != null) ? Config.SiteSettings.PageSize : 10;
				// Extract a portion of data
				var pageData = await knowledgeOpsIQ.Skip(firstItem).Take(pageSize).ToListAsync();
				if (pageData == null || pageData.Count() == 0)
					return StatusCode(204);  // 204 := "No Content"
				var content = await _renderer.RenderPartialToStringAsync("~/Pages/Experience/_Index.cshtml", pageData);
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
