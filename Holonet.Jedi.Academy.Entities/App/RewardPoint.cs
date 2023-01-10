using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.App
{
	public class RewardPoint
	{
		public int Id { get; set; }

		[Required]
		public int StudentId { get; set; }
		public Student? Student { get; set; }

		public int? KnowledgeId { get; set; }
		public Knowledge? Knowledge { get; set; }

		public int? QuestId { get; set; }
		public Quest? Quest { get; set; }

	}
}
