using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Models;
using System.ComponentModel.DataAnnotations;
using FoolProof.Core;
using Holonet.Jedi.Academy.BL.Dashboards;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Holonet.Jedi.Academy.App.Pages.Quests
{
	[Authorize(Roles = "Administrator, Instructor")]
	public class CreateModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		private string userOffset;

		public CreateModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			userOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString();
			_userManager = userManager;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.RankLevel).ToArrayAsync(), "Id", "Name");
			ViewData["Planets"] = new MultiSelectList(await _context.Planets.OrderBy(x => x.Name).ToArrayAsync(), "Id", "Name");
			GetDashboardItems();
			return Page();
		}

		[BindProperty]
		[Required]
		public int[] SelectedDestinationIds { get; set; } = default!;

		[BindProperty]
		public QuestVM Quest { get; set; } = default!;

		public double NewQuestsToday { get; set; }

		public double QuestsCompletedToday { get; set; }

		public double AvgCompletionTime { get; set; }

		public string MostPopularPlanet { get; set; } = string.Empty;


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!await CanCreateEditItem())
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new Exception("You are not permitted to perform the previous operation."));
			}
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var emptyQuest = new Quest();
			emptyQuest.Populate(Quest);
			emptyQuest.CreatedOn = DateTime.Now;
			_context.Quests.Add(emptyQuest);
			await _context.SaveChangesAsync();
			return RedirectToPage("Edit", new { id = emptyQuest.Id });
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}

		private async void GetDashboardItems()
		{
			QuestDashboard d = new QuestDashboard(Config, userOffset);

			NewQuestsToday = await _context.Quests.Where(x => x.CreatedOn.Date.Equals(DateTime.Now.Date)).CountAsync();

			QuestsCompletedToday = await _context.QuestParticipation.Where(x => x.CompletedOn.HasValue && x.CompletedOn.Value.Date.Equals(DateTime.Now.Date)).CountAsync();

			AvgCompletionTime = d.GetQuestAvgCompletion(await _context.QuestParticipation.Where(x => x.CompletedOn.HasValue).ToListAsync());
			
			MostPopularPlanet = d.GetMostPopularPlanet(await _context.ObjectiveDestinations.Include(q => q.Planet).ToListAsync());
		}
	}
}
