using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]	
	public class QuestsController : ControllerBase
	{
		private readonly ILogger<QuestsController> _logger;
		private readonly AcademyContext _context;

		public QuestsController(ILogger<QuestsController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Quest>>> GetQuests()
		{
			return await _context.Quests
				.Include(q=>q.Objectives).ThenInclude(qo=>qo.Objective).ThenInclude(o=>o.Destinations).ThenInclude(d=>d.Planet)
				.Include(q=>q.Rank)
				.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Quest>> GetQuest(int id)
		{
			var quest = await _context.Quests
				.Include(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.Include(q => q.Rank)
				.Where(q => q.Id == id)
				.FirstOrDefaultAsync();

			if (quest == null)
			{
				return NotFound();
			}

			return quest;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutQuest(int id, Quest item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			_context.Quests.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!QuestExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/[controller]
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Quest>> PostQuest(Quest item)
		{
			_context.Quests.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetQuest", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Quest>> DeleteQuest(int id)
		{
			var quest = await _context.Quests.FindAsync(id);
			if (quest == null)
			{
				return NotFound();
			}

			_context.Quests.Remove(quest);
			await _context.SaveChangesAsync();

			return quest;
		}

		private bool QuestExists(int id)
		{
			return _context.Quests.Any(e => e.Id == id);
		}
	}
}