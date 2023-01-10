using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Pages.References.Ranks
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

		public IList<Rank> Ranks { get; set; } = default!;

		[BindProperty]
		public Rank Rank { get; set; } = default!;

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

			if (_context.Ranks != null)
			{
				Ranks = await _context.Ranks.OrderBy(x => x.RankLevel).ToListAsync();
				if (id.HasValue)
				{
					ID = id.Value;
					Rank = await _context.Ranks.FindAsync(id);
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

			var emptyRank = new Rank();

			if (await TryUpdateModelAsync<Rank>(
				emptyRank,
				"rank",   // Prefix for form value.
				s => s.Name, s => s.Minimum, s => s.Maximum, s => s.RankLevel, s => s.Archived))
			{
				if (id.HasValue)
				{
					ID = id.Value;
					_context.Attach(Rank).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				else
				{
					if (_context.Ranks.Any(x => x.Name.Equals(emptyRank.Name)))
						return StatusCode(StatusCodes.Status500InternalServerError, new Exception("An item with this name already exists."));
					_context.Ranks.Add(emptyRank);
					await _context.SaveChangesAsync();
					ID = emptyRank.Id;
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
