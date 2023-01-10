using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.App.Areas.Identity.Enums;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Pages.Experience
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

		public Knowledge Knowledge { get; set; }
		public int ID { get; set; }
		public KnowledgeXP KnowledgeParticipation { get; set; }

		public bool CanJoin { get; set; } = false;
		public bool CanCreateEdit { get; set; } = false;

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.KnowledgeOpportunities == null)
            {
                return NotFound();
            }

			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				throw new Exception("A valid user account was not detected.");
			}
			ID = id.Value;
			CanCreateEdit = await CanCreateEditItem();

			var knowledge = await _context.KnowledgeOpportunities.FirstOrDefaultAsync(m => m.Id == id);
            if (knowledge == null)
            {
                return NotFound();
            }
            else 
            {
                Knowledge = knowledge;
				//CanJoin = EvaluateKnowOpEligibility(currentUser, Knowledge);
			}
			KnowledgeParticipation = await _context.KnowledgeLearned
				.Where(x => x.KnowledgeId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser)))
				.FirstOrDefaultAsync();
			CanJoin = (KnowledgeParticipation == null) ? true : false;
			return Page();
        }

		public async Task<IActionResult> OnPostCompleteObjectiveAsync(int? id)
		{
			if (id == null || _context.KnowledgeLearned == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}
			KnowledgeParticipation = await _context.KnowledgeLearned.Where(x => x.KnowledgeId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser))).FirstOrDefaultAsync();
			if (KnowledgeParticipation != null)
			{
				KnowledgeParticipation.CompletedOn = DateTime.Now;
				_context.KnowledgeLearned.Update(KnowledgeParticipation);
				await _context.SaveChangesAsync();
			}
			else
			{
				//throw error, not participating yet
			}
			return RedirectToPage("Details", new { id = ID });
		}

		public async Task<IActionResult> OnPostJoinAsync(int? id)
		{
			if (id == null || _context.KnowledgeOpportunities == null || _context.KnowledgeLearned == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}
			var knowledgeOp = await _context.KnowledgeOpportunities.FirstOrDefaultAsync(m => m.Id == id);
			if (knowledgeOp == null)
			{
				return NotFound();
			}
			KnowledgeParticipation = await _context.KnowledgeLearned.Where(x => x.KnowledgeId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser))).FirstOrDefaultAsync();
			CanJoin = (KnowledgeParticipation == null) ? true : false;
			if (CanJoin)
			{
				var emptyKnowledgeParticipation = new KnowledgeXP();
				emptyKnowledgeParticipation.KnowledgeId = ID;
				emptyKnowledgeParticipation.StartedOn = DateTime.Now;
				emptyKnowledgeParticipation.StudentId = GetStudentId(currentUser);
				emptyKnowledgeParticipation.ExperienceToGain = knowledgeOp.ExperienceToGain;

				_context.KnowledgeLearned.Add(emptyKnowledgeParticipation);
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
			if (id == null || _context.KnowledgeLearned == null)
			{
				return NotFound();
			}
			ID = id.Value;
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}

			KnowledgeParticipation = await _context.KnowledgeLearned.Where(x => x.KnowledgeId.Equals(id) && x.StudentId.Equals(GetStudentId(currentUser))).FirstOrDefaultAsync();
			if (KnowledgeParticipation != null)
			{
				_context.KnowledgeLearned.Remove(KnowledgeParticipation);
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

		//private bool EvaluateKnowOpEligibility(UserAccount currentUser, Knowledge aKnowledge)
		//{
		//	bool canParticipate = false;
		//	var userProfileDomain = _context.UserProfiles.Include(x => x.Student).Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefault();
		//	if (userProfileDomain != null && userProfileDomain.Student != null && aKnowledge != null)
		//	{
		//		if (userProfileDomain.Student.Rank.RankLevel >= aQuest.Rank.RankLevel)
		//		{
		//			canParticipate = true;
		//		}
		//	}

		//	return canParticipate;
		//}
	}
}
