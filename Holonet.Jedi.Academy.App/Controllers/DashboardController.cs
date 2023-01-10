using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.BL.Dashboards;
using Newtonsoft.Json.Linq;
using Holonet.Jedi.Academy.BL.Data;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.App.Controllers
{
	[Produces("application/json")]
	[Route("api/dashboard")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	[ApiController]
	public class DashboardController : ControllerDefaults
	{
		private IMemoryCache SiteCache;
		private string userOffset;
		private readonly AcademyContext _context;

		public DashboardController(IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, AcademyContext context) : base(options, httpContextAccessor)
		{
			SiteCache = memoryCache;
			userOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString();
			_context = context;
		}

		[Route("GetQuestParticipation")]
		[HttpPost]
		public async Task<IActionResult> GetQuestParticipation([FromBody] JObject data)
		{
			if (data == null || data["questId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int questId = data["questId"].ToObject<int>();

				try
				{
					QuestDashboard d = new QuestDashboard(Config, userOffset);
					var questParticipation = await _context.QuestParticipation.Where(x => x.QuestId.Equals(questId)).ToListAsync();
					return Ok(d.GetQuestParticipation(questParticipation, "Quest Participation"));
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the quest participation.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		[Route("GetQuestAvgCompletion")]
		[HttpPost]
		public async Task<IActionResult> GetQuestAvgCompletion([FromBody] JObject data)
		{
			if (data == null || data["questId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int questId = data["questId"].ToObject<int>();

				try
				{
					QuestDashboard d = new QuestDashboard(Config, userOffset);
					var questParticipation = await _context.QuestParticipation.Where(x => x.QuestId.Equals(questId) && x.CompletedOn.HasValue).ToListAsync();
					return Ok(d.GetQuestAvgCompletion(questParticipation));
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the requested avg quest completion report.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		[Route("GetSkillParticipation")]
		[HttpPost]
		public async Task<IActionResult> GetSkillParticipation([FromBody] JObject data)
		{
			if (data == null || data["knowledgeId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int knowledgeId = data["knowledgeId"].ToObject<int>();

				try
				{
					KnowledgeDashboard d = new KnowledgeDashboard(Config, userOffset);
					var knowledgeParticipation = await _context.KnowledgeLearned.Where(x => x.KnowledgeId.Equals(knowledgeId)).ToListAsync();
					return Ok(d.GetSkillParticipation(knowledgeParticipation, "Experience Participation"));
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the experience participation.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		[Route("GetSkillAvgCompletion")]
		[HttpPost]
		public async Task<IActionResult> GetSkillAvgCompletion([FromBody] JObject data)
		{
			if (data == null || data["knowledgeId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int knowledgeId = data["knowledgeId"].ToObject<int>();

				try
				{
					KnowledgeDashboard d = new KnowledgeDashboard(Config, userOffset);
					var knowledgeParticipation = await _context.KnowledgeLearned.Where(x => x.KnowledgeId.Equals(knowledgeId) && x.CompletedOn.HasValue).ToListAsync();
					return Ok(d.GetSkillAvgCompletion(knowledgeParticipation));
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the requested avg quest completion report.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
	}
}
