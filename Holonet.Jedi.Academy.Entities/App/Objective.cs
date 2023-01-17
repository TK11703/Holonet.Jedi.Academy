using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Holonet.Jedi.Academy.Entities.Models;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class Objective
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
		[Display(Name = "Name")]
		public string Name { get; set; } = string.Empty;

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		[Required]
		[Display(Name = "Planets")]
		public List<ObjectiveDestination> Destinations { get; set; }

		[Required]
		[Display(Name = "Archived")]
		public bool Archived { get; set; }

		public Objective()
		{
			Destinations = new List<ObjectiveDestination>();
		}

		public List<Planet> GetDestinationPlanets()
		{
			if(Destinations != null)
			{
				return Destinations.Where(x=>x.Planet != null).Select(x=>x.Planet!).ToList();
			}
			else
			{
				return new List<Planet>();
			}
		}

		public void Populate(ObjectiveVM objective)
		{
			this.Id = objective.Id;
			this.Name = objective.Name;
			this.Description = objective.Description;
			this.Archived = objective.Archived;
		}
	}
}
