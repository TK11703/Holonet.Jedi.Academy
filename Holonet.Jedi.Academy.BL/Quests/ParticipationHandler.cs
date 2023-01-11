using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.BL.Quests
{
	public class ParticipationHandler
	{
		private readonly AcademyContext _context;
		public ParticipationHandler(AcademyContext context)
		{
			_context = context;
		}

		public async Task<bool> MarkAsCompleteAsync(QuestXP item)
		{
			bool completed = false;
			item.CompletedOn = DateTime.Now;
			_context.QuestParticipation.Update(item);
			if(await _context.SaveChangesAsync() > 0)
				completed= true;
			return completed;
		}

		public async Task<bool> MarkAsCompleteAsync(KnowledgeXP item)
		{
			bool completed = false;
			item.CompletedOn = DateTime.Now;
			_context.KnowledgeLearned.Update(item);
			if (await _context.SaveChangesAsync() > 0)
				completed = true;
			return completed;
		}

		public async Task<bool> UpdateExperienceAsync(QuestXP item, int studentId)
		{
			bool completed = false;
			var student = await _context.Students.FindAsync(studentId);
			if(student != null && item != null && !item.AddedToStudent)
			{
				student.Experience += item.ExperienceToGain;
				_context.Students.Update(student);
				item.AddedToStudent = true;
				_context.QuestParticipation.Update(item);
				Notification newNotif = new Notification();
				newNotif.StudentId = studentId;
				newNotif.SentOn = DateTime.Now;
				newNotif.Message = string.Format("Congratulations, you completed the Quest '{0}'!", item.Quest.Name);
				_context.Notifications.Add(newNotif);
				if (await _context.SaveChangesAsync() > 0)
					completed = true;
			}
			return completed;
		}

		public async Task<bool> UpdateExperienceAsync(KnowledgeXP item, int studentId)
		{
			bool completed = false;
			var student = await _context.Students.FindAsync(studentId);
			if (student != null && item != null && item.Knowledge != null && !item.AddedToStudent)
			{
				student.Experience += item.ExperienceToGain;
				_context.Students.Update(student);
				item.AddedToStudent = true;
				_context.KnowledgeLearned.Update(item);
				Notification newNotif = new Notification();
				newNotif.StudentId = studentId;
				newNotif.SentOn = DateTime.Now;
				newNotif.Message = string.Format("Congratulations, you completed the experience '{0}'!", item.Knowledge.Name);
				_context.Notifications.Add(newNotif);
				if (await _context.SaveChangesAsync() > 0)
					completed = true;
			}
			return completed;
		}
	}
}
