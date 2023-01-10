using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Models
{
    public class UserProfileVM
    {
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Species")]
		public int SpeciesId { get; set; }

		[Required]
		[Display(Name = "Home Planet")]
		public int PlanetId { get; set; }

		public void Populate(UserProfile profile)
		{
			this.Id = profile.Id;
			this.UserId = profile.UserId;
			if (profile.Student != null)
			{
				this.FirstName = profile.Student.FirstName;
				this.LastName = profile.Student.LastName;
				this.SpeciesId = profile.Student.SpeciesId;
				this.PlanetId = profile.Student.PlanetId;
			}
		}
	}
}
