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
using Holonet.Jedi.Academy.Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Holonet.Jedi.Academy.App.Pages.References.ForcePowers
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

		public IList<ForcePower> ForcePowers { get;set; } = default!;

		[BindProperty]
		public ForcePowerVM ForcePower { get; set; } = default!;

		public ForcePower ForcePowerDomain { get; set; } = default!;

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
			ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.RankLevel).ToArrayAsync(), "Id", "Name");
			if (_context.ForcePowers != null)
            {
				ForcePowers = await _context.ForcePowers.OrderBy(x=>x.Name).ToListAsync();
				if (id.HasValue)
				{
					ID = id.Value;
					ForcePowerDomain = await _context.ForcePowers.FindAsync(id);
					ForcePower = new ForcePowerVM();
					ForcePower.Populate(ForcePowerDomain);
				}
			}            
        }

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if(!await CanCreateEditItem())
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new Exception("You are not permitted to perform the previous operation."));
			}
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var emptyForcePower = new ForcePower();
			emptyForcePower.Populate(ForcePower);
			if (id.HasValue)
			{
				ID = id.Value;
				_context.Attach(emptyForcePower).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			else
			{
				if (_context.ForcePowers.Any(x => x.Name.Equals(emptyForcePower.Name)))
					return StatusCode(StatusCodes.Status500InternalServerError, new Exception("An item with this name already exists."));
				_context.ForcePowers.Add(emptyForcePower);
				await _context.SaveChangesAsync();
				ID = emptyForcePower.Id;
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
