using Holonet.Jedi.Academy.Entities;
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
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Pages.RoleManager
{
    [Authorize(Roles = "Administrator")]
    public partial class RoleModel : RazorPageDefaults
    {
        private readonly UserManager<JediAcademyAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleModel> _logger;

        public RoleModel(RoleManager<IdentityRole> roleManager,
            ILogger<RoleModel> logger,
            UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public RoleInputModel Input { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
        {
            Input = new RoleInputModel();
            if (!ID.Equals("-1"))
            {
                IdentityRole role = await _roleManager.FindByIdAsync(ID);
                if (role != null)
                {
                    Input.Name = role.Name;
                }
                else
                {
                    ModelState.AddModelError("", "No role found");
                }
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return SendPostErrorReply();
            }
            IdentityResult result = null;
            if (ID.Equals("-1"))
            {
                result = await _roleManager.CreateAsync(new IdentityRole(Input.Name));
            }
            else
            {
                IdentityRole role = await _roleManager.FindByIdAsync(ID);
                if (role != null)
                {
                    role.Name = Input.Name;
                    result = await _roleManager.UpdateAsync(role);
                }
                else
                {
                    ModelState.AddModelError("", "No role found");
                }
            }
            if (result.Succeeded)
                return RedirectToPage("Index");
            else
            {
                IdentityErrors(result);
                return Page();
            }
        }

        private IActionResult IdentityErrors(IdentityResult result)
        {
            var allErrors = result.Errors.SelectMany(v => v.Description);
            if (allErrors != null && allErrors.Count() > 0)
            {
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = Environment.NewLine + "The data sent to the server was invalid." + Environment.NewLine + string.Join(Environment.NewLine, allErrors)
                });
            }
            else
            {
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = Environment.NewLine + "The data sent to the server was invalid." + Environment.NewLine + "Please check your form inputs for issues."
                });
            }
        }

        private IActionResult SendPostErrorReply()
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            if (allErrors != null && allErrors.Count() > 0)
            {
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = Environment.NewLine + "The data sent to the server was invalid." + Environment.NewLine + string.Join(Environment.NewLine, allErrors)
                });
            }
            else
            {
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = Environment.NewLine + "The data sent to the server was invalid." + Environment.NewLine + "Please check your form inputs for issues."
                });
            }
        }

        [BindProperty(SupportsGet = true)]
        public string ID { get; set; } = string.Empty;
    }
}
