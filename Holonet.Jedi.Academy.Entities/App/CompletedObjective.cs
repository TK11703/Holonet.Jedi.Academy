using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class CompletedObjective
	{
		public int Id { get; set; }

		[Required]
		public int QuestXPId { get; set; }
		public QuestXP? QuestXP { get; set; }  

		[Required]
		public int ObjectiveId { get; set; }
		public Objective? Objective { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Completed On")]
		public DateTime CompletedOn { get; set; }

		public CompletedObjective()
		{
		}
	}
}
