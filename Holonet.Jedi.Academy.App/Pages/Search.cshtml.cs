using Holonet.Jedi.Academy.BL.Search;
using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Pages
{
    public class SearchModel : RazorPageDefaults
    {
        private readonly ILogger<SearchModel> _logger;
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;

		public SearchModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor, ILogger<SearchModel> logger): base(options, httpContextAccessor)
        {
            _logger = logger;
			_context = context;
		}

		public string NameSort { get; set; }
		public string TypeSort { get; set; }
		public string CurrentSort { get; set; }
		public string SearchString { get; set; }

		public PaginatedList<SearchResult> Results { get; set; }

		public async Task OnGetAsync(string search, string sortOrder, int? pageIndex)
		{
			if (String.IsNullOrEmpty(sortOrder))
				sortOrder = "Name";
			CurrentSort = sortOrder;
			SearchString = search;
			NameSort = sortOrder == "Name" ? "name_desc" : "Name";
			TypeSort = sortOrder == "Type" ? "type_desc" : "Type";
			SearchHandler searching = new SearchHandler(_context);
			IQueryable<SearchResult> resultsIQ = searching.ExecuteSearch(search);
			switch (sortOrder)
			{
				case "Name":
					resultsIQ = resultsIQ.OrderBy(s => s.Name);
					break;
				case "name_desc":
					resultsIQ = resultsIQ.OrderByDescending(s => s.Name);
					break;
				case "Type":
					resultsIQ = resultsIQ.OrderBy(s => s.Type);
					break;
				case "type_desc":
					resultsIQ = resultsIQ.OrderByDescending(s => s.Type);
					break;
				default:
					resultsIQ = resultsIQ.OrderBy(s => s.Name);
					break;
			}

			var pageSize = Config.SiteSettings.PageSize;
			Results = await PaginatedList<SearchResult>.CreateAsync(resultsIQ, pageIndex ?? 1, pageSize);
		}
	}
}