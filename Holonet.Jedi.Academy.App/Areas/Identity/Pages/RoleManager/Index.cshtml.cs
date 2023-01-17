using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Pages.RoleManager
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

        public IActionResult OnGet()
        {
            Roles = _roleManager.Roles;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (!string.IsNullOrEmpty(roleId))
            {
                IdentityRole role = await _roleManager.FindByIdAsync(roleId);
                ModelState.AddModelError("", "Cannot remove user existing roles");
                if (role != null)
                {
                    await _roleManager.DeleteAsync(role);
                }
            }
            Roles = _roleManager.Roles;
            return Page();
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IEnumerable<IdentityRole> Roles { get; set; } = default!;
	}
}
