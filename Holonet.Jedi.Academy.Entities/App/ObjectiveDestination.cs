using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class ObjectiveDestination
	{
        public int Id { get; set; }

		[Required]
		[Display(Name = "Planet")]
		public int PlanetId { get; set; } = 0;
		public Planet? Planet { get; set; }

		[Required]
		[Display(Name = "Objective")]
		public int ObjectiveId { get; set; } = 0;
		public Objective? Objective { get; set; }

		public ObjectiveDestination() 
		{ 
		}
	}
}
