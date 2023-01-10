using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Data;

// Add profile data for application users by adding properties to the JediAcademyAppUser class
public class JediAcademyAppUser : IdentityUser
{
    [Required(ErrorMessage = "A first name is required."), Display(Name = "First Name:")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "A last name is required."), Display(Name = "Last Name:")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    public int UsernameChangeLimit { get; set; } = 10;

    public byte[]? ProfilePicture { get; set; }

    [NotMapped]
    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
}

