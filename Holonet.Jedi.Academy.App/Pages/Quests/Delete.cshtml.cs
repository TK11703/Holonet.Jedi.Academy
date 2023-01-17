using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Holonet.Jedi.Academy.App.Pages.Quests
{
	[Authorize(Roles = "Administrator, Instructor")]
	public class DeleteModel : PageModel
    {
        private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;

        public DeleteModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Quest Quest { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Quests == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests
                .Include(q => q.Rank)
				.Include(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.FirstOrDefaultAsync(m => m.Id == id);

            if (quest == null)
            {
                return NotFound();
            }
            else 
            {
                Quest = quest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Quests == null)
            {
                return NotFound();
            }
            var quest = await _context.Quests.FindAsync(id);

            if (quest != null)
            {
                Quest = quest;
                _context.Quests.Remove(Quest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
