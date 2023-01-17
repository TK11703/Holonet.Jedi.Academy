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

namespace Holonet.Jedi.Academy.App.Pages.Experience
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
      public Knowledge Knowledge { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.KnowledgeOpportunities == null)
            {
                return NotFound();
            }

            var knowledge = await _context.KnowledgeOpportunities.FirstOrDefaultAsync(m => m.Id == id);

            if (knowledge == null)
            {
                return NotFound();
            }
            else 
            {
                Knowledge = knowledge;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.KnowledgeOpportunities == null)
            {
                return NotFound();
            }
            var knowledge = await _context.KnowledgeOpportunities.FindAsync(id);

            if (knowledge != null)
            {
                Knowledge = knowledge;
                _context.KnowledgeOpportunities.Remove(Knowledge);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
