using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.App
{
	public class Notification
	{
		public int Id { get; set; }

		[Required]
		public int StudentId { get; set; }
		public Student? Student { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Sent On")]
		public DateTime SentOn { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Acknowledged On")]
		public DateTime AcknowledgedOn { get; set; }

		[Required]
		[Display(Name = "Acknowledged")]
		public bool Acknowledged { get; set; }

		[MaxLength(1000, ErrorMessage = "Your description is too long, the maximum is 1000 characters.")]
		[Display(Name = "Message")]
		public string? Message { get; set; }
	}
}
