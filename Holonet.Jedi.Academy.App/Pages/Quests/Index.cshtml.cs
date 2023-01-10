using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;

namespace Holonet.Jedi.Academy.App.Pages.Quests
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

		public bool CanCreateEdit { get; set; } = false;

		public async Task OnGetAsync()
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}

			CanCreateEdit = await CanCreateEditItem();

			ViewData["Ranks"] = new SelectList(await _context.Ranks.OrderBy(x => x.RankLevel).ToListAsync(), "Id", "Name");
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}
	}
}
