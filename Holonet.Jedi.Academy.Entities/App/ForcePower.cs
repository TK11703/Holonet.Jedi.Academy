using Holonet.Jedi.Academy.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class ForcePower
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		public int? MinimumRankId { get; set; }
		public Rank? MinimumRank{ get; set; }

		[Required]
		[Display(Name = "Archived")]
		public bool Archived { get; set; }

		public void Populate(ForcePowerVM powerVM)
		{
			this.Id = powerVM.Id;
			this.Name = powerVM.Name;
			this.Description = powerVM.Description;
			this.MinimumRankId = powerVM.MinimumRankId;
			this.Archived = powerVM.Archived;
		}
	}
}
