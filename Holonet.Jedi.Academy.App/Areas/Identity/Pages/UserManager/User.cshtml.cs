using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.App.Areas.Identity.Models;
using Holonet.Jedi.Academy.App.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Pages.UserManager
{
    [Authorize(Roles = "Administrator")]
    public partial class UserModel : RazorPageDefaults
    {
        private readonly UserManager<JediAcademyAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserModel> _logger;

        public UserModel(RoleManager<IdentityRole> roleManager,
            ILogger<UserModel> logger,
            UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public List<ManageUserRolesViewModel> UserRoleAssignment { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                ModelState.AddModelError("", $"User with Id = {ID} cannot be found");
                return Page();
            }
            ViewData["UserName"] = user.UserName;
            UserRoleAssignment = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                UserRoleAssignment.Add(userRolesViewModel);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return Page();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }
            result = await _userManager.AddToRolesAsync(user, UserRoleAssignment.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }
            return RedirectToPage("Index");            
        }

        [BindProperty(SupportsGet = true)]
        public string ID { get; set; } = String.Empty;
    }
}
