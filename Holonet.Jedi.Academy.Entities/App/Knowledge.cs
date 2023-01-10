using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Holonet.Jedi.Academy.Entities.Models;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class Knowledge
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 1)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        [Display(Name = "Experience To Gain")]
        public int ExperienceToGain { get; set; }

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		public string? ShortDescription
		{
			get
			{
				if (Description != null)
				{
					if (Description.Length > 150)
						return Description.Substring(0, 150) + "...";
					else
						return Description;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		[Required]
		[Display(Name = "Archived")]
		public bool Archived { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Available On")]
		public DateTime CreatedOn { get; set; }

		[NotMapped]
		public bool HasUserCompleted { get; set; } = false;

		[NotMapped]
		public bool CanUserCreateEdit { get; set; } = false;

		public Knowledge()
		{

		}

		public void Populate(KnowledgeVM knowledgeVM)
		{
			this.Id = knowledgeVM.Id;
			this.Name = knowledgeVM.Name;
			this.ExperienceToGain = knowledgeVM.ExperienceToGain;
			this.Description = knowledgeVM.Description;
			this.Archived = knowledgeVM.Archived;
		}
	}
}
