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

namespace Holonet.Jedi.Academy.App.Pages.References.Planets
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

		public IList<Planet> Planets { get; set; } = default!;

		[BindProperty]
		public Planet Planet { get; set; } = default!;

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

			if (_context.Planets != null)
			{
				Planets = await _context.Planets.OrderBy(x => x.Name).ToListAsync();
				if (id.HasValue)
				{
					ID = id.Value;
					Planet = await _context.Planets.FindAsync(id);
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

			var emptyPlanet = new Planet();

			if (await TryUpdateModelAsync<Planet>(
				emptyPlanet,
				"planet",   // Prefix for form value.
				s => s.Name, s => s.Archived))
			{
				if (id.HasValue)
				{
					ID = id.Value;
					_context.Attach(Planet).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				else
				{
					if (_context.Planets.Any(x => x.Name.Equals(emptyPlanet.Name)))
						return StatusCode(StatusCodes.Status500InternalServerError, new Exception("An item with this name already exists."));
					_context.Planets.Add(emptyPlanet);
					await _context.SaveChangesAsync();
					ID = emptyPlanet.Id;
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
