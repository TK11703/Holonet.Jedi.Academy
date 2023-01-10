using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Holonet.Jedi.Academy.Entities.App;

namespace Holonet.Jedi.Academy.Entities.Models
{
	public class QuestVM
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

		[Required]
		[Display(Name = "Minimum Rank")]
		public int RankId { get; set; }

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		[Required]
		[Display(Name = "Archived")]
		public bool Archived { get; set; }

		public void Populate(Quest aQuest)
		{
			this.Id = aQuest.Id;
			this.Name = aQuest.Name;
			this.RankId = aQuest.RankId;
			this.ExperienceToGain = aQuest.ExperienceToGain;
			this.Description = aQuest.Description;
			this.Archived = aQuest.Archived;
		}
	}
}
