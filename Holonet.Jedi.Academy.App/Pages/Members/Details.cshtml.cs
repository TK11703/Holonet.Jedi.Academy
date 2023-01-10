using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Pages.Members
{
	public class DetailsModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public DetailsModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
		}

		public Student Student { get; set; }

		public List<QuestXP> PersonalQuests { get; set; }

		public List<KnowledgeXP> PersonalSkills { get; set; }

		public List<ForcePowerXP> ForcePowers { get; set; }

		public bool CanCreateEdit { get; set; } = false;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _context.Students == null)
			{
				return NotFound();
			}
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}
			CanCreateEdit = await CanCreateEditItem();

			var student = await _context.Students
				.Include(x => x.Planet)
				.Include(x => x.Rank)
				.Include(x => x.Species)
				.Include(x => x.ReasonForTermination)
				.Include(s => s.Quests).ThenInclude(qxp => qxp.Quest).ThenInclude(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.Include(s => s.Knowledge).ThenInclude(kxp => kxp.Knowledge)
				.Include(s => s.ForcePowers).ThenInclude(fxp => fxp.ForcePower)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (student == null)
			{
				return NotFound();
			}
			else
			{
				Student = student;
				PersonalQuests = student.Quests;
				PersonalSkills = student.Knowledge;
				ForcePowers = student.ForcePowers;
			}
			return Page();
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
