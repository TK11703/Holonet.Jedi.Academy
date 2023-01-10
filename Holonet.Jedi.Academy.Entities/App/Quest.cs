using Holonet.Jedi.Academy.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
	public class Quest
	{
		public int Id { get; set; }

		[Required]
		[StringLength(250, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 1)]
		[Display(Name = "Name")]
		public string Name { get; set; } = string.Empty;

		[Display(Name = "Objective(s)")]
		public List<QuestObjective> Objectives { get; set; }

		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
		[Display(Name = "Experience To Gain")]
		public int ExperienceToGain { get; set; }

		[Required]
		[Display(Name = "Minimum Rank")]
		public int RankId { get; set; }

		[Display(Name = "Minimum Rank")]
		public Rank? Rank { get; set; }

		public int MinimumRankLevel
		{
			get
			{
				return (Rank != null) ? Rank.RankLevel : 1;
			}
		}

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		public string? ShortDescription { 
			get 
			{ 
				if(Description != null)
				{
					if(Description.Length > 150) 
						return Description.Substring(0,150) + "...";
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

		public Quest()
		{
			Objectives = new List<QuestObjective>();
		}

		public List<Planet> DestinationPlanets()
		{
			List<Planet> destinations = new List<Planet>();
			if (Objectives != null)
			{
				foreach (var objective in Objectives)
				{
					destinations.AddRange(objective.DestinationPlanets());
				}
			}
			return destinations.DistinctBy(x=>x.Id).OrderBy(x=>x.Name).ToList();
		}

		public void Populate(QuestVM questVM)
		{
			this.Id = questVM.Id;
			this.Name = questVM.Name;
			this.RankId = questVM.RankId;
			this.ExperienceToGain = questVM.ExperienceToGain;
			this.Description = questVM.Description;
			this.Archived = questVM.Archived;
		}
	}
}
