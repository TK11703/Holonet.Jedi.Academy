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

        public int? RankId { get; set; }
        public Rank? Rank { get; set; }

	}
}
