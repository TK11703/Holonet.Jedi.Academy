using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class QuestObjective
	{
        public int Id { get; set; }

		[Required]
        [Display(Name = "Objective")]
        public int ObjectiveId { get; set; } = 0;
		public Objective? Objective { get; set; }

		[Required]
		[Display(Name = "Quest")]
		public int QuestId { get; set; } = 0;
		public Quest? Quest { get; set; }

		public QuestObjective()
		{
		}

		public List<Planet> DestinationPlanets() 
		{
			if (Objective != null)
			{
				return Objective.GetDestinationPlanets();
			}
			else
			{
				return new List<Planet>();
			}
		}
	}
}
