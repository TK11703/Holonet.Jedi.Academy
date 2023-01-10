using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Holonet.Jedi.Academy.App.Pages
{
    public class ThrowErrorModel : PageModel
    {
        public void OnGet()
        {
            throw new Exception("This is an exception that is thrown on purpose.", new Exception("This is the real reason why it was thrown..."));
        }
    }
}