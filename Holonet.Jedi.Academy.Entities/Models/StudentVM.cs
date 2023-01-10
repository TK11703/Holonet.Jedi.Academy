using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holonet.Jedi.Academy.Entities.App;

namespace Holonet.Jedi.Academy.Entities.Models
{
    public class StudentVM
    {
		public int Id { get; set; }

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

        [Required]
        [Display(Name = "Rank")]
        public int RankId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Initiated On")]
        public DateTime? InitiatedOn { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        public int Experience { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Left On")]
        public DateTime? LeftOn { get; set; }

        [Display(Name = "Reason for termination")]
        public int? ReasonForTerminationId { get; set; }

        public void Populate(Student student)
		{
			this.Id = student.Id;
            this.FirstName = student.FirstName;
            this.LastName = student.LastName;
            this.SpeciesId = student.SpeciesId;
            this.PlanetId = student.PlanetId;
            this.RankId = student.RankId;
            this.Experience= student.Experience;
            this.InitiatedOn= student.InitiatedOn;
            this.LeftOn= student.LeftOn;
            this.ReasonForTerminationId= student.ReasonForTerminationId;
        }
	}
}
