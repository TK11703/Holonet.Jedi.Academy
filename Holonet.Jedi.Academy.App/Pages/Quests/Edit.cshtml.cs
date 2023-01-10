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
using System.ComponentModel.DataAnnotations;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Holonet.Jedi.Academy.App.Pages.Quests
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
        public QuestVM Quest { get; set; } = default!;

		public Quest QuestDomain { get; set; } = default!;


		[BindProperty]
        [Required]
        public int[] SelectedObjectiveIds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			if (id == null || _context.Quests == null || !await _context.Quests.AnyAsync(x=>x.Id.Equals(id.Value)))
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

			if (id == null || _context.Quests == null || !await _context.Quests.AnyAsync(x => x.Id.Equals(id.Value)))
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
            {
                return Page();
            }
			QuestDomain = await _context.Quests
				.Include(q => q.Rank)
				.Include(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.FirstOrDefaultAsync(m => m.Id.Equals(id));
            QuestDomain.Populate(Quest);

			QuestDomain.Objectives.Clear();
            if(SelectedObjectiveIds != null && SelectedObjectiveIds.Count()> 0)
            {
				foreach (int objectiveId in SelectedObjectiveIds)
				{
					QuestDomain.Objectives.Add(new QuestObjective() { QuestId = Quest.Id, ObjectiveId = objectiveId });
				}
			}

            _context.Update(QuestDomain);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await QuestExistsAsync(Quest.Id))
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
			QuestDomain = await _context.Quests
				.Include(q => q.Rank)
				.Include(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.FirstOrDefaultAsync(m => m.Id.Equals(id));
            Quest = new QuestVM();
            Quest.Populate(QuestDomain);
            SelectedObjectiveIds = QuestDomain.Objectives.Select(x => x.ObjectiveId).ToArray();
			ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.RankLevel).ToArrayAsync(), "Id", "Name", Quest.RankId);
			ViewData["Objectives"] = new SelectList(await _context.Objectives.Where(x => !x.Archived).OrderBy(x => x.Name).ToArrayAsync(), "Id", "Name");
		}

		private async Task<bool> QuestExistsAsync(int id)
        {
          return await _context.Quests.AnyAsync(e => e.Id.Equals(id));
        }
		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
