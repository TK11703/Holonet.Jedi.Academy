using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Holonet.Jedi.Academy.App.Pages.Errors
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class SessionExpiredModel : PageModel
    {
        private readonly ILogger _logger;

        public SessionExpiredModel(ILogger<SessionExpiredModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string code)
        {
            
        }
    }
}