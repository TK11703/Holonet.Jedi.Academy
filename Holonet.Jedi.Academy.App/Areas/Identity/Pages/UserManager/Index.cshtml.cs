using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Pages.UserManager
{
    [Authorize(Roles = "Administrator")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<JediAcademyAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<JediAcademyAppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public List<UserRolesViewModel> RoleCollection { get; set; } = default!;

		public async Task<IActionResult> OnGet()
        {
            var users = await _userManager.Users.ToListAsync();
            RoleCollection = new List<UserRolesViewModel>();
            foreach (JediAcademyAppUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.IsLocked = await IsUserLockedoutAsync(user.Email);
                thisViewModel.Roles = await GetUserRoles(user);
                RoleCollection.Add(thisViewModel);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                JediAcademyAppUser me = _userManager.GetUserAsync(User).Result;
                if (me.Id.Equals(userId))
                {
                    ModelState.AddModelError("", "Cannot remove yourself while logged in.");
                }
                else
                {
                    var user = _userManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
                    if (user != null)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", "A user was not found with the ID passed.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "A user ID was not found in the request.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUnlockUserAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userManager.FindByIdAsync(userId).Result;
                if(user != null)
                {
                    var result = await _userManager.SetLockoutEndDateAsync(user, null);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Unable to unlock the requested user at this time.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "A user was not found with the ID passed.");
                }
            }
            else
            {
                ModelState.AddModelError("", "A user ID was not found in the request.");
            }
            return Page();
        }

        private async Task<bool> IsUserLockedoutAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return await _userManager.IsLockedOutAsync(user);
            }
            else
            {
                return false;
            }
        }

        private async Task<List<string>> GetUserRoles(JediAcademyAppUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public List<UserRolesViewModel> UserRolesViewModel { get; set; } = default!;
	}
}
