using System.Collections.Generic;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public bool IsLocked { get; set; } = false;
        public IEnumerable<string> Roles { get; set; }

        public UserRolesViewModel()
        {
            Roles = new List<string>();
        }
    }
}
