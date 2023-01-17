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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Holonet.Jedi.Academy.Entities;
using FoolProof.Core;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using Holonet.Jedi.Academy.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Holonet.Jedi.Academy.App.Pages.References.QuestObjectives
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
			SelectedDestinationIds = new int[0];
		}
		public string NameSort { get; set; } = string.Empty;
		public string SearchString { get; set; } = string.Empty;
		public string CurrentSort { get; set; } = string.Empty;
		public PaginatedList<Objective> QuestObjectives { get; set; } = default!;

		[BindProperty]
		public ObjectiveVM QuestObjective { get; set; } = default!;

		public Objective QuestObjectiveDomain { get; set; } = default!;

		[BindProperty]
		[Required]
		[Display(Name = "Planets")]
		public int[] SelectedDestinationIds { get; set; }

		public int ID { get; set; }
		public bool CanCreateEdit { get; set; } = false;

		public async Task OnGetAsync(int? id, string sortOrder, string searchString, int? pageIndex)
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}
			CanCreateEdit = await CanCreateEditItem();

			await PrepareModelDataAsync(id, sortOrder, searchString, pageIndex);
		}

		public async Task<IActionResult> OnPostAsync(int? id, string sortOrder, string? searchString, int? pageIndex)
		{
			if (!await CanCreateEditItem())
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new Exception("You are not permitted to perform the previous operation."));
			}
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (id.HasValue &&  id.Value > 0)
			{
				ID = id.Value;
				QuestObjectiveDomain = await _context.Objectives.Include(o => o.Destinations).ThenInclude(d => d.Planet).Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
				QuestObjectiveDomain.Populate(QuestObjective);
				QuestObjectiveDomain.Destinations.Clear();
				foreach (int planetId in SelectedDestinationIds)
				{
					QuestObjectiveDomain.Destinations.Add(new ObjectiveDestination() { PlanetId = planetId, ObjectiveId = ID });
				}
				_context.Update(QuestObjectiveDomain);
				await _context.SaveChangesAsync();
			}
			else
			{
				var emptyObjective = new Objective();
				if (await TryUpdateModelAsync<Objective>(
				emptyObjective,
				"questobjective",   // Prefix for form value.
				s => s.Name, s => s.Description, s => s.Archived))
				{

					if (_context.Objectives.Any(x => x.Name.Equals(emptyObjective.Name)))
						return StatusCode(StatusCodes.Status500InternalServerError, new Exception("An item with this name already exists."));
					foreach (int planetId in SelectedDestinationIds)
					{
						emptyObjective.Destinations.Add(new ObjectiveDestination() { PlanetId = planetId });
					}
					_context.Objectives.Add(emptyObjective);
				}
				await _context.SaveChangesAsync();
				ID = emptyObjective.Id;
			}

			return RedirectToPage("Index", new { Id = ID });
		}

		private async Task PrepareModelDataAsync(int? id, string sortOrder, string? searchString, int? pageIndex)
		{
			if (String.IsNullOrEmpty(sortOrder))
				sortOrder = "Name";
			CurrentSort = sortOrder;
			NameSort = sortOrder == "Name" ? "name_desc" : "Name";
			SearchString = searchString;
			if (_context.QuestObjectives != null)
			{
				IQueryable<Objective> objectivesIQ = _context.Objectives;
				if (!String.IsNullOrEmpty(searchString))
				{
					objectivesIQ = objectivesIQ.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || (x.Description != null && x.Description.ToUpper().Contains(searchString.ToUpper())));
				}
				switch (sortOrder)
				{
					case "Name":
						objectivesIQ = objectivesIQ.OrderBy(s => s.Name);
						break;
					case "name_desc":
						objectivesIQ = objectivesIQ.OrderByDescending(s => s.Name);
						break;
					default:
						objectivesIQ = objectivesIQ.OrderBy(s => s.Name);
						break;
				}

				var pageSize = (Config.SiteSettings != null) ? Config.SiteSettings.PageSize : 10;
				QuestObjectives = await PaginatedList<Objective>.CreateAsync(objectivesIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
				if (id.HasValue)
				{
					ID = id.Value;
					QuestObjectiveDomain = await _context.Objectives.Include(o => o.Destinations).ThenInclude(d=>d.Planet).Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
					if (QuestObjectiveDomain != null)
					{
						QuestObjective = new ObjectiveVM();
						QuestObjective.Populate(QuestObjectiveDomain);
						SelectedDestinationIds = QuestObjectiveDomain.Destinations.Select(x => x.PlanetId).ToArray();
					}
					ViewData["Planets"] = new MultiSelectList(await _context.Planets.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", SelectedDestinationIds);
				}
				else
				{
					ViewData["Planets"] = new MultiSelectList(await _context.Planets.OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
				}
			}
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
