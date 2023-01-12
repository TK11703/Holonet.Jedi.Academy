using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.Data.SqlClient.DataClassification;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities.Models;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Holonet.Jedi.Academy.BL.Search;
using Holonet.Jedi.Academy.BL.Quests;

namespace Holonet.Jedi.Academy.App.Pages.Quests
{
	public class DetailsModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;
		private readonly UserManager<JediAcademyAppUser> _userManager;

		public DetailsModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, UserManager<JediAcademyAppUser> userManager, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
		}

		public Quest Quest { get; set; }

		public int ID { get; set; }

		public QuestXP QuestParticipation { get; set; }

		public bool CanJoin { get; set; } = false;

		public bool CanCreateEdit { get; set; } = false;

		[BindProperty]
		public int[] CompletedObjectiveIds { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _context.Quests == null)
			{
				return NotFound();
			}
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}

			ID = id.Value;
			CanCreateEdit = await CanCreateEditItem();
			var quest = await _context.Quests
				.Include(q => q.Rank)
				.Include(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (quest == null)
			{
				return NotFound();
			}
			else
			{
				Quest = quest;
				CanJoin = EvaluateQuestEligibility(currentUser, quest);
			}
			QuestParticipation = await _context.QuestParticipation
				.Include(qp => qp.CompletedObjectives)
				.Include(qp => qp.Quest).ThenInclude(q => q.Objectives)
				.Where(x => x.QuestId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser)))
				.FirstOrDefaultAsync();
			if (QuestParticipation != null)
			{
				CompletedObjectiveIds = QuestParticipation.CompletedObjectives.Select(x => x.ObjectiveId).ToArray();
			}
			else
			{
				CompletedObjectiveIds = new int[0];
			}
			return Page();
		}

		public async Task<IActionResult> OnPostCompleteObjectiveAsync(int? id)
		{
			if (id == null || _context.Quests == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}
			int studentId = GetStudentId(currentUser);
			QuestParticipation = await _context.QuestParticipation
				.Include(qp => qp.CompletedObjectives)
				.Include(qp => qp.Quest).ThenInclude(q => q.Objectives)
				.Where(x => x.QuestId.Equals(id) && x.StudentId.Equals(studentId))
				.FirstOrDefaultAsync();
			if (QuestParticipation != null)
			{
				bool updateNeeded = false;
				foreach (int objectiveId in CompletedObjectiveIds)
				{
					if (!QuestParticipation.CompletedObjectives.Exists(x => x.ObjectiveId.Equals(objectiveId) && x.QuestXPId.Equals(QuestParticipation.Id)))
					{
						QuestParticipation.CompletedObjectives.Add(new CompletedObjective { ObjectiveId = objectiveId, QuestXPId = QuestParticipation.Id, CompletedOn = DateTime.Now });
						updateNeeded = true;
					}
				}
				if (updateNeeded)
				{
					_context.QuestParticipation.Update(QuestParticipation);
					await _context.SaveChangesAsync();
				}
				if (QuestParticipation.PercentComplete.Equals(100))
				{
					ParticipationHandler quests = new ParticipationHandler(_context);
					if (!await quests.MarkAsCompleteAsync(QuestParticipation))
					{
						throw new Exception("Unable to mark this quest as complete.");
					}
					if (!await quests.UpdateExperienceAsync(QuestParticipation, studentId))
					{
						throw new Exception("Unable to update the experience value for the user.");
					}
					RankHandler ranking = new RankHandler(_context);
					if (await ranking.IsEligible(studentId))
					{
						await ranking.RankUp(studentId);
					}
				}
			}
			else
			{
				throw new Exception("You are not participating in this quest yet.");
			}
			return RedirectToPage("Details", new { id = ID });
		}

		public async Task<IActionResult> OnPostJoinAsync(int? id)
		{
			if (id == null || _context.Quests == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}

			var quest = await _context.Quests
				.Include(q => q.Rank)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (quest == null)
			{
				return NotFound();
			}
			else
			{
				CanJoin = EvaluateQuestEligibility(currentUser, quest);
			}
			if (CanJoin)
			{
				var emptyQuestParticipation = new QuestXP();
				emptyQuestParticipation.QuestId = ID;
				emptyQuestParticipation.StartedOn = DateTime.Now;
				emptyQuestParticipation.StudentId = GetStudentId(currentUser);
				emptyQuestParticipation.ExperienceToGain = quest.ExperienceToGain;

				_context.QuestParticipation.Add(emptyQuestParticipation);
				await _context.SaveChangesAsync();
			}
			else
			{
				//throw error, can't participate yet
			}
			return RedirectToPage("Details", new { id = ID });
		}

		public async Task<IActionResult> OnPostLeaveAsync(int? id)
		{
			if (id == null || _context.QuestParticipation == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}

			QuestParticipation = await _context.QuestParticipation.Where(x => x.QuestId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser))).FirstOrDefaultAsync();
			if (QuestParticipation != null)
			{
				_context.QuestParticipation.Remove(QuestParticipation);
				await _context.SaveChangesAsync();
			}
			else
			{
				//throw error, not participating, so can't leave
			}
			return RedirectToPage("Details", new { id = ID });
		}

		private async Task<bool> CanCreateEditItem()
		{
			JediAcademyAppUser user = await _userManager.GetUserAsync(User);
			return await _userManager.IsInRoleAsync(user, Roles.Administrator.ToString());
		}

		private int GetStudentId(UserAccount currentUser)
		{
			int studentId = -1;
			var userProfileDomain = _context.UserProfiles.Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefault();
			if (userProfileDomain != null)
			{
				studentId = userProfileDomain.StudentId;
			}
			return studentId;
		}

		private bool EvaluateQuestEligibility(UserAccount currentUser, Quest aQuest)
		{
			bool canParticipate = false;
			var userProfileDomain = _context.UserProfiles.Include(x => x.Student).ThenInclude(s => s.Rank).Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefault();
			if (userProfileDomain != null && userProfileDomain.Student != null && userProfileDomain.Student.Rank != null && aQuest != null && aQuest.Rank != null)
			{
				if (userProfileDomain.Student.Rank.RankLevel >= aQuest.Rank.RankLevel)
				{
					canParticipate = true;
				}
			}

			return canParticipate;
		}
	}
}
