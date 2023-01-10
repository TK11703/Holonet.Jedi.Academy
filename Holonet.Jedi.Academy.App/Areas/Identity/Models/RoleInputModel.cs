using System.ComponentModel.DataAnnotations;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Models
{
    public class RoleInputModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = String.Empty;
    }
}
