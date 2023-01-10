using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Pages.Experience
{
	[Authorize(Roles = "Administrator, Instructor")]
	public class CreateModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public CreateModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
		}

		public bool CanCreateEdit { get; set; } = false;

		public async Task<IActionResult> OnGetAsync()
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}

			CanCreateEdit = await CanCreateEditItem();
			return Page();
		}

		[BindProperty]
		public Knowledge Knowledge { get; set; }


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var emptyKnowledge = new Knowledge();

			if (await TryUpdateModelAsync<Knowledge>(
				emptyKnowledge,
				"knowledge",   // Prefix for form value.
				s => s.Name, s => s.ExperienceToGain, s => s.Description))
			{
				_context.KnowledgeOpportunities.Add(emptyKnowledge);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
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
