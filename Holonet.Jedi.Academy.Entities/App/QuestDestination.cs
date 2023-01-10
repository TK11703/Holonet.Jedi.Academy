using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class QuestDestination
	{
        public int Id { get; set; }

		[Required]
        [Display(Name = "Planet")]
        public int PlanetId { get; set; }
		public Planet? Planet { get; set; }

		[Required]
        [Display(Name = "Quest")]
        public int QuestId { get; set; }
		public Quest? Quest { get; set; }

	}
}
