using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using System.Net.NetworkInformation;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Pages.References.AlienRaces
{
	[Authorize(Roles = "Administrator")]
	public class IndexModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public IndexModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
		}

		public IList<Species> AlienRaces { get; set; } = default!;

		[BindProperty]
		public Species AlienRace { get; set; } = default!;

		public int ID { get; set; }
		public bool CanCreateEdit { get; set; } = false;

		public async Task OnGetAsync(int? id)
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}
			CanCreateEdit = await CanCreateEditItem();

			if (_context.AlienRaces != null)
			{
				AlienRaces = await _context.AlienRaces.OrderBy(x => x.Name).ToListAsync();
				if (id.HasValue)
				{
					ID = id.Value;
					AlienRace = await _context.AlienRaces.FindAsync(id);
				}
			}
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (!await CanCreateEditItem())
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new Exception("You are not permitted to perform the previous operation."));
			}
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var emptySpecies = new Species();

			if (await TryUpdateModelAsync<Species>(
				emptySpecies,
				"alienrace",   // Prefix for form value.
				s => s.Name, s => s.Archived))
			{
				if (id.HasValue)
				{
					ID = id.Value;
					_context.Attach(AlienRace).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				else
				{
					if (_context.AlienRaces.Any(x => x.Name.Equals(emptySpecies.Name)))
						return StatusCode(StatusCodes.Status500InternalServerError, new Exception("An item with this name already exists."));
					_context.AlienRaces.Add(emptySpecies);
					await _context.SaveChangesAsync();
					ID = emptySpecies.Id;
				}
			}

			return RedirectToPage("Index", new { Id = ID });
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
