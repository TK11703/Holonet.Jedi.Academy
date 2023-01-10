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

namespace Holonet.Jedi.Academy.App.Pages.Members
{
	[Authorize(Roles = "Administrator")]
	public class EditModel : PageModel
    {
        private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;

        public EditModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentVM Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(x => x.Planet)
                .Include(x => x.Rank)
                .Include(x => x.Species)
                .Include(x => x.ReasonForTermination)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            Student = new StudentVM();
            Student.Populate(student);
            ViewData["Planets"] = new SelectList(await _context.Planets.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", Student.PlanetId);
            ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", Student.RankId);
            ViewData["Species"] = new SelectList(await _context.AlienRaces.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", Student.SpeciesId);
            ViewData["TerminationReasons"] = new SelectList(await _context.TerminationReasons.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", Student.ReasonForTerminationId);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Student studentDomain = await _context.Students
                .Include(x => x.Planet)
                .Include(x => x.Rank)
                .Include(x => x.Species)
                .Include(x => x.ReasonForTermination)
                .FirstOrDefaultAsync(m => m.Id == Student.Id);
            if (studentDomain == null)
            {
                return NotFound();
            }
            studentDomain.Populate(Student);

            _context.Attach(studentDomain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExists(Student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> StudentExists(int id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}
