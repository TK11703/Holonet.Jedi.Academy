using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Holonet.Jedi.Academy.App.Pages
{
	public class UserProfileModel : RazorPageDefaults
	{
		private readonly Holonet.Jedi.Academy.BL.Data.AcademyContext _context;

		public UserProfileModel(Holonet.Jedi.Academy.BL.Data.AcademyContext context, IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
		{
			_context = context;
		}

		[BindProperty]
		public UserProfileVM UserProfile { get; set; } = default!;

		public Student Student { get; set; } = default!;

		public List<QuestXP> PersonalQuests { get; set; } = default!;

		public List<KnowledgeXP> PersonalSkills { get; set; } = default!;

		public List<ForcePowerXP> ForcePowers { get; set; } = default!;

		public List<RewardPoint> RewardPoints { get; set; } = default!;

		public string RewardPointOutput
		{
			get
			{
				if(RewardPoints != null && RewardPoints.Count > 0)
				{
					if (RewardPoints.Count > 99)
						return "99+";
					else
						return RewardPoints.Count.ToString();
				}
				else
				{
					return "0";
				}
			}
		}

		public int StudentId { get; set; }

		[BindProperty]
		[Required]
		[Display(Name = "Skill Selection")]
		public int SelectedForcePowerId { get; set; }

		public string ButtonAction { get; set; } = string.Empty;

		public async Task<IActionResult> OnGetAsync()
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}
			UserProfile = new UserProfileVM();
			UserProfile userProfileDomain = new Entities.UserProfile();
			if (await UserProfileExists(currentUser.UserId))
			{
				userProfileDomain = await _context.UserProfiles
					.Include(x=>x.Student).ThenInclude(s=>s.Quests).ThenInclude(qxp=>qxp.Quest).ThenInclude(q=>q.Objectives).ThenInclude(qo=>qo.Objective).ThenInclude(o=>o.Destinations).ThenInclude(d=>d.Planet)
					.Include(x=>x.Student).ThenInclude(s=>s.Knowledge).ThenInclude(kxp=>kxp.Knowledge)
					.Include(x => x.Student).ThenInclude(s => s.ForcePowers).ThenInclude(fxp => fxp.ForcePower)
					.Include(x => x.Student).ThenInclude(s => s.Rank)
					.Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefaultAsync();
				if (userProfileDomain != null)
				{
					UserProfile.Populate(userProfileDomain);
				}
				StudentId = userProfileDomain.StudentId;
				if(StudentId > 0)
				{
					Student = await _context.Students.FindAsync(StudentId);
				}
				else
				{
					Student = new Student() { Id = 0 };
				}
				RewardPoints = await _context.RewardPoints.Where(x => x.StudentId.Equals(StudentId)).ToListAsync();				
				ButtonAction = "Update";
				List<int> excludeIds = userProfileDomain.Student.ForcePowers.Select(x=>x.ForcePowerId).ToList();
				ViewData["ForcePowers"] = new SelectList(await _context.ForcePowers
					.Include(fp=>fp.MinimumRank)
					.Where(x=> !x.Archived && !excludeIds.Contains(x.Id) && userProfileDomain.Student.Rank.RankLevel >= x.MinimumRank.RankLevel)
					.OrderBy(x => x.Name)
					.ToListAsync(), "Id", "Name");
			}
			else
			{
				StudentId = 0;
				Student = new Student() { Id=0 };
				UserProfile.UserId = currentUser.UserId;
				UserProfile.FirstName = currentUser.FirstName;
				UserProfile.LastName = currentUser.LastName;
				ButtonAction = "Save";
				ViewData["ForcePowers"] = new SelectList(new List<ForcePower>(), "Id", "Name");
			}
			ViewData["Planets"] = new SelectList(await _context.Planets.Where(x=>!x.Archived).OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
			ViewData["Species"] = new SelectList(await _context.AlienRaces.Where(x => !x.Archived).OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
			PopulatePersonalProperties(userProfileDomain);
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.GetFieldValidationState("UserProfile") != ModelValidationState.Valid)
			{
				return Page();
			}
			if (await UserProfileExists(UserProfile.Id))
			{
				ButtonAction = "Update";
				var profileToUpdate = await _context.UserProfiles.FindAsync(UserProfile.Id);
				if (profileToUpdate == null)
				{
					return NotFound();
				}
				profileToUpdate.Student.PlanetId = UserProfile.PlanetId;
				profileToUpdate.Student.SpeciesId = UserProfile.SpeciesId;
				_context.Update(profileToUpdate);

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await UserProfileExists(UserProfile.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}
			else
			{
				ButtonAction = "Save";
				var emptyUserProfile = new UserProfile();
				emptyUserProfile.UserId = UserProfile.UserId;
				emptyUserProfile.Student = new Student();
				emptyUserProfile.Student.FirstName= UserProfile.FirstName;
				emptyUserProfile.Student.LastName= UserProfile.LastName;
				emptyUserProfile.Student.SpeciesId= UserProfile.SpeciesId;
				emptyUserProfile.Student.PlanetId = UserProfile.PlanetId;
				emptyUserProfile.Student.Experience = 0;
				emptyUserProfile.Student.InitiatedOn = DateTime.Now;
				emptyUserProfile.Student.Rank = await _context.Ranks.Where(x => x.RankLevel.Equals(1)).FirstOrDefaultAsync();
				if (emptyUserProfile.Student.Rank != null)
				{
					emptyUserProfile.Student.RankId = emptyUserProfile.Student.Rank.Id;
				}
				
				_context.UserProfiles.Add(emptyUserProfile);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("UserProfile");
		}

		public async Task<IActionResult> OnPostNewSkillAsync(int studentId)
		{
			UserAccount? currentUser = GetActiveUser();
			if (currentUser == null)
			{
				return NotFound();
			}
			if (studentId > 0 && SelectedForcePowerId > 0)
			{
				var forcePower = await _context.ForcePowers.Include(fp => fp.MinimumRank).Where(x => !x.Archived && x.Id.Equals(SelectedForcePowerId)).FirstOrDefaultAsync();
				if (forcePower == null)
				{
					throw new Exception("The force power was invalid.");
				}
				var student = await _context.Students.Include(s => s.Rank).Where(x => x.Id.Equals(studentId)).FirstOrDefaultAsync();
				if (student == null)
				{
					throw new Exception("The student identifier was invalid.");
				}
				if (_context.ForcePowersLearned.Any(x=>x.ForcePowerId.Equals(SelectedForcePowerId) && x.StudentId.Equals(studentId)))
				{
					throw new Exception("The student has already obtained this force power.");
				}
				if (student.Rank.RankLevel < forcePower.MinimumRank.RankLevel)
				{
					throw new Exception("The student does not currently meet the minimum rank level for to learn this force power. Please try again later.");
				}
				var rewardPoint = await _context.RewardPoints.Where(x => x.StudentId.Equals(studentId)).FirstOrDefaultAsync();
				if (rewardPoint == null)
				{
					throw new Exception("The student does not currently possess any reward points. Please try again later.");
				}
				_context.RewardPoints.Remove(rewardPoint);
				_context.ForcePowersLearned.Add(new ForcePowerXP { ForcePowerId = SelectedForcePowerId, StudentId = studentId, GainedOn = DateTime.Now });
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new Exception("The student or skill were invalid. Please try again.");
			}

			return RedirectToPage("UserProfile");
		}

		private async Task<bool> UserProfileExists(int id)
		{
			return await _context.UserProfiles.AnyAsync(e => e.Id == id);
		}

		private async Task<bool> UserProfileExists(string userId)
		{
			return await _context.UserProfiles.AnyAsync(e => e.UserId.Equals(userId));
		}

		private void PopulatePersonalProperties(Entities.UserProfile userProfile)
		{
			PersonalQuests = userProfile.Student.Quests;
			PersonalSkills = userProfile.Student.Knowledge;
			ForcePowers = userProfile.Student.ForcePowers;
		}
	}
}
