using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.BL.Quests
{
	public class RankHandler
	{
		private readonly AcademyContext _context;
		public RankHandler(AcademyContext context)
		{
			_context = context;
		}

		public async Task<bool> IsEligible(int studentId)
		{
			bool eligible = false;
			var student = await _context.Students.Include(s => s.Rank).Where(x => x.Id.Equals(studentId)).FirstOrDefaultAsync();
			if (student != null)
			{
				if(student.Rank != null && student.Experience > student.Rank.Maximum)
					eligible = true;
			}
			else
			{
				throw new Exception("Unable to find a student by that ID.");
			}
			return eligible;
		}

		public async Task<bool> RankUp(int studentId)
		{
			bool completed = false;
			if(await IsEligible(studentId))
			{
				var student = await _context.Students.Include(s => s.Rank).Where(x=>x.Id.Equals(studentId)).FirstOrDefaultAsync();
				if(student != null && student.Rank != null)
				{
					var newRank = _context.Ranks.Where(x => x.RankLevel > student.Rank.RankLevel).OrderBy(x => x.RankLevel).FirstOrDefault();
					if (newRank != null)
					{
						student.RankId = newRank.Id;
						student.Rank = newRank;
						_context.Students.Update(student);
						var rewardPoint = new RewardPoint();
						rewardPoint.StudentId = studentId;
						rewardPoint.RankId = newRank.Id;
						_context.RewardPoints.Add(rewardPoint);
						Notification newNotif = new Notification();
						newNotif.StudentId = studentId;
						newNotif.SentOn = DateTime.Now;
						newNotif.Message = string.Format("You have ranked up! Your new level is: {0}. A reward point has been added in your profile.", newRank.Name);
						_context.Notifications.Add(newNotif);
						if (await _context.SaveChangesAsync() > 0)
							completed = true;
					}
				}
				else
				{
					throw new Exception("Unable to find a student by that ID.");
				}
			}
			else
			{
				throw new Exception("The indicated student has not yet met the criteria for a new rank.");
			}

			return completed;
		}

	}
}
