namespace Holonet.Jedi.Academy.App.Areas.Identity.Models
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }

        public ManageUserRolesViewModel()
        {
            RoleId = string.Empty;
            RoleName = string.Empty;
        }
    }    
}
