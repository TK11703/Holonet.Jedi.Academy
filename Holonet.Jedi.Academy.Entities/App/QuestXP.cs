using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
	public class QuestXP
	{
		public int Id { get; set; }

		[Required]
		public int QuestId { get; set; }
		public Quest Quest { get; set; }

		[Required]
		public int StudentId { get; set; }
		public Student? Student { get; set; }

		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
		[Display(Name = "Experience To Gain")]
		public int ExperienceToGain { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Started On")]
		public DateTime StartedOn { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Completed On")]
		public DateTime? CompletedOn { get; set; }

		public List<CompletedObjective> CompletedObjectives { get; set; }

		public bool AddedToStudent { get; set; } = false;

		public string Status
		{
			get
			{
				return (CompletedOn.HasValue) ? "Completed" : "Not Complete";
			}
		}

		public bool IsComplete
		{
			get
			{
				return (CompletedOn.HasValue) ? true : false;
			}
		}

		public double Rotations
		{
			get
			{
				var end = DateTime.Now;
				if (CompletedOn.HasValue)
					end = CompletedOn.Value;
				return (end.Date - StartedOn.Date).TotalDays;
			}
		}

		public double PercentComplete
		{
			get
			{
				if (IsComplete)
				{
					return 100;
				}
				else
				{
					if (Quest != null && Quest.Objectives != null & Quest.Objectives.Count > 0 && CompletedObjectives.Count > 0)
					{
						return Math.Round((((double)CompletedObjectives.Count / Quest.Objectives.Count) * 100),0);
					}
					else
						return 0;
				}
			}
		}

		public bool IsObjectiveComplete(int objectiveId)
		{
			return CompletedObjectives.Exists(x=>x.ObjectiveId.Equals(objectiveId) && x.QuestXPId.Equals(this.Id));
		}

		public QuestXP()
		{
			CompletedObjectives = new List<CompletedObjective>();
		}
	}
}
