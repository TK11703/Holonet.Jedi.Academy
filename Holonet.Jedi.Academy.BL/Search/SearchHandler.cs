using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.BL.Search
{
	public class SearchHandler
	{
		private readonly AcademyContext _context;
		public SearchHandler(AcademyContext context) {
			_context = context;
		}

		public IQueryable<SearchResult> ExecuteSearch(string searchString)
		{
			IQueryable<SearchResult> resultsIQ = from q in _context.Quests
												 where q.Name.Contains(searchString) || q.Description.Contains(searchString)
												 select new SearchResult() {Id=q.Id, Name=q.Name, Type="Quests" };

			resultsIQ = resultsIQ.Union(from k in _context.KnowledgeOpportunities
						where k.Name.Contains(searchString) || k.Description.Contains(searchString)
						select new SearchResult() { Id = k.Id, Name = k.Name, Type = "Experience" });

			return resultsIQ.AsNoTracking();
		}
	}
}
