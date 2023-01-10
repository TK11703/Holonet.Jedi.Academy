using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Models;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Pages.Experience
{
	[Authorize(Roles = "Administrator, Instructor")]
	public class EditModel : RazorPageDefaults
    {
        private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public EditModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
		}

		[BindProperty]
        public KnowledgeVM Knowledge { get; set; } = default!;

		public Knowledge KnowledgeDomain { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.KnowledgeOpportunities == null)
            {
                return NotFound();
            }

			await GetModelDataAsync(id);

			return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
			if (!await CanCreateEditItem())
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new Exception("You are not permitted to perform the previous operation."));
			}

			if (id == null || _context.KnowledgeOpportunities == null || !await _context.KnowledgeOpportunities.AnyAsync(x => x.Id.Equals(id.Value)))
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return Page();
			}
			KnowledgeDomain = await _context.KnowledgeOpportunities.FirstOrDefaultAsync(m => m.Id.Equals(id));
			KnowledgeDomain.Populate(Knowledge);

			_context.Update(KnowledgeDomain);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await KnowledgeExistsAsync(Knowledge.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Page();
		}

		private async Task GetModelDataAsync(int? id)
		{
			KnowledgeDomain = await _context.KnowledgeOpportunities.FirstOrDefaultAsync(m => m.Id.Equals(id));
			Knowledge = new KnowledgeVM();
			Knowledge.Populate(KnowledgeDomain);
		}

		private async Task<bool> KnowledgeExistsAsync(int id)
        {
          return await _context.KnowledgeOpportunities.AnyAsync(e => e.Id == id);
        }

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
