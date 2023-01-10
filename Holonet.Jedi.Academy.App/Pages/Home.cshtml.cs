using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Pages
{
    public class HomeModel : RazorPageDefaults
    {
        private IMemoryCache SiteCache;
        private readonly ILogger<HomeModel> _logger;
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;

		public IList<Quest> Quests { get; set; } = default!;

        public bool CompleteProfile { get; set; }

        public string ProfileIssuesJSON { get; set; }

        public HomeModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, ILogger<HomeModel> logger, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(options, httpContextAccessor)
        {
            _logger = logger;
            SiteCache = memoryCache;
            _context= context;
        }

        public async Task OnGetAsync()
        {
            IQueryable<Quest> questsIQ = from r in _context.Quests
                                         .Include(q => q.Rank)
                                         orderby r.Rank.RankLevel, r.Name
                                         select r;
            Quests = await questsIQ.Take(6).AsNoTracking().ToListAsync();
            UserAccount? currentUser = GetActiveUser();
            if (currentUser != null)
            {
                if (_context.UserProfiles.Any(x => x.UserId.Equals(currentUser.UserId)))
                {
                    UserProfile currentUserProfile = await _context.UserProfiles.Include(x=>x.Student).Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefaultAsync();
                    if (currentUserProfile != null) 
                    { 
                        ProfileChecks(currentUserProfile); 
                    }
                }
            }

        }
        private void ProfileChecks(UserProfile profile)
        {
            if (profile != null)
            {
                List<string> issues = profile.GetProfileIssues();
                if (issues != null && issues.Count > 0)
                {
                    CompleteProfile = false;
                    var data = new { issues = issues };
                    ProfileIssuesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
                else
                {
                    CompleteProfile = true;
                }
            }
            else
            {
                CompleteProfile = false;
                var data = new { issues = new List<string> { "A valid profile was not retrieved from the system." } };
                ProfileIssuesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }
    }
}