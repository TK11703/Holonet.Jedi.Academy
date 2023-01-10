using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;

namespace Holonet.Jedi.Academy.App.Pages.Members
{
    public class IndexModel : RazorPageDefaults
    {
        private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public IndexModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
            _context = context;
			_userManager = userManager;
		}

        public string NameSort { get; set; }
        public string SpeciesSort { get; set; }
        public string PlanetSort { get; set; }
        public string ExpSort { get; set; }
        public string RankSort { get; set; }
        public string NameFilter { get; set; }
		public int? RankFilter { get; set; }
		public int? SpeciesFilter { get; set; }
		public int? PlanetFilter { get; set; }

		public string CurrentSort { get; set; }

        public List<string> SearchFilterDesc { get; set; }

        public PaginatedList<Student> Students { get; set; } = default!;

		public bool CanCreateEdit { get; set; } = false;

		public async Task OnGetAsync(string sortOrder, string nameFilter, int? rankFilter, int? speciesFilter, int? planetFilter, int? pageIndex)
        {
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
                throw new Exception("A valid user account was not detected.");
			}

            CanCreateEdit = await CanCreateEditItem();

			SearchFilterDesc = new List<string>();
            if (String.IsNullOrEmpty(sortOrder))
                sortOrder = "Name";
            CurrentSort = sortOrder;
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            SpeciesSort = sortOrder == "Species" ? "species_desc" : "Species";
            PlanetSort = sortOrder == "Planet" ? "planet_desc" : "Planet";
            ExpSort = sortOrder == "Experience" ? "exp_desc" : "Experience";
            RankSort = sortOrder == "Rank" ? "rank_desc" : "Rank";

            //if (searchString != null)
            //{
            //    pageIndex = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            NameFilter = nameFilter;
            RankFilter = rankFilter;
            PlanetFilter = planetFilter;
            SpeciesFilter = speciesFilter;
			ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.RankLevel).ToListAsync(), "Id", "Name", rankFilter);
			ViewData["Planets"] = new SelectList(await _context.Planets.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", planetFilter);
			ViewData["Species"] = new SelectList(await _context.AlienRaces.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", speciesFilter);
			IQueryable<Student> studentsIQ = from s in _context.Students
                                         .Include(s => s.Planet)
                                         .Include(s => s.Rank)
                                         .Include(s => s.Species)
                                         select s;
            if (!String.IsNullOrEmpty(nameFilter))
            {
                SearchFilterDesc.Add(string.Format("<strong>Filter first or last name with:</strong> '{0}'", nameFilter));
                studentsIQ = studentsIQ.Where(x => x.LastName.ToUpper().Contains(nameFilter.ToUpper())
                || x.FirstName.ToUpper().Contains(nameFilter.ToUpper()));
            }

			if (rankFilter != null)
			{
				studentsIQ = studentsIQ.Where(x => x.RankId.Equals(rankFilter));
				var selectedRank = await _context.Ranks.Where(x => x.Id.Equals(rankFilter)).FirstOrDefaultAsync();
				if (selectedRank != null)
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter rank with:</strong> '{0}'", selectedRank.Name));
				}
				else
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter rank with:</strong> '{0}'", "Unknown"));
				}
			}

			if (planetFilter != null)
			{
				studentsIQ = studentsIQ.Where(x => x.PlanetId.Equals(planetFilter));
				var selectedPlanet = await _context.Planets.Where(x => x.Id.Equals(planetFilter)).FirstOrDefaultAsync();
				if (selectedPlanet != null)
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter planet with:</strong> '{0}'", selectedPlanet.Name));
				}
				else
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter planet with:</strong> '{0}'", "Unknown"));
				}
			}

			if (speciesFilter != null)
			{
				studentsIQ = studentsIQ.Where(x => x.SpeciesId.Equals(speciesFilter));
				var selectedSpecies = await _context.AlienRaces.Where(x => x.Id.Equals(speciesFilter)).FirstOrDefaultAsync();
				if (selectedSpecies != null)
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter species with:</strong> '{0}'", selectedSpecies.Name));
				}
				else
				{
					SearchFilterDesc.Add(string.Format("<strong>Filter species with:</strong> '{0}'", "Unknown"));
				}
			}

			switch (sortOrder)
            {
                case "Name":
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Species":
                    studentsIQ = studentsIQ.OrderBy(s => s.Species.Name);
                    break;
                case "species_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Species.Name);
                    break;
                case "Planet":
                    studentsIQ = studentsIQ.OrderBy(s => s.Planet.Name);
                    break;
                case "planet_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Planet.Name);
                    break;
                case "Experience":
                    studentsIQ = studentsIQ.OrderBy(s => s.Experience);
                    break;
                case "exp_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Experience);
                    break;
                case "Rank":
                    studentsIQ = studentsIQ.OrderBy(s => s.Rank.RankLevel);
                    break;
                case "rank_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Rank.RankLevel);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            var pageSize = Config.SiteSettings.PageSize;
            Students = await PaginatedList<Student>.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
