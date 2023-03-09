using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<JediAcademyAppUser> _userManager;
        private readonly SignInManager<JediAcademyAppUser> _signInManager;

        public ConfirmEmailChangeModel(UserManager<JediAcademyAppUser> userManager, SignInManager<JediAcademyAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; } = String.Empty;

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => "<p>" + x.Description + "</p>");
				StatusMessage = "<h3>Error changing email.</h3>" + Environment.NewLine + string.Join(Environment.NewLine, errors);
                return Page();
            }
            if (IsValidEmail(user.UserName))
            {
                //if the username is an email address, then we need to update it as well.
                var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
                if (!setUserNameResult.Succeeded)
                {
					var errors = result.Errors.Select(x => "<p>" + x.Description + "</p>");
					StatusMessage = "<h3>Error changing user name.</h3>" + Environment.NewLine + string.Join(Environment.NewLine, errors);
					return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }

		private bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
