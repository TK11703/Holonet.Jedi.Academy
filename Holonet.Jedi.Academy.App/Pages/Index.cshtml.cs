using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Pages
{
    public class IndexModel : RazorPageDefaults
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor, ILogger<IndexModel> logger): base(options, httpContextAccessor)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
			SessionHandler sessionHandler = new SessionHandler(CurrentHttpContext.HttpContext);
			UserSession? sessionObj = sessionHandler.GetSession();
			if (sessionObj != null && sessionObj.HasAcknowledgedConsent)
			{
				return RedirectToPage("./Home");
			}

			return Page();
		}
    }
}