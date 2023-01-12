using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]	
	public class KnowledgeController : ControllerBase
	{
		private readonly ILogger<KnowledgeController> _logger;
		private readonly AcademyContext _context;

		public KnowledgeController(ILogger<KnowledgeController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Knowledge>>> GetKnowledges()
		{
			return await _context.KnowledgeOpportunities.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Knowledge>> GetKnowledge(int id)
		{
			var Knowledge = await _context.KnowledgeOpportunities
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (Knowledge == null)
			{
				return NotFound();
			}

			return Knowledge;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutKnowledge(int id, Knowledge item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			_context.KnowledgeOpportunities.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!KnowledgeExists(id))
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
		public async Task<ActionResult<Knowledge>> PostKnowledge(Knowledge item)
		{
			_context.KnowledgeOpportunities.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetKnowledge", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Knowledge>> DeleteKnowledge(int id)
		{
			var Knowledge = await _context.KnowledgeOpportunities.FindAsync(id);
			if (Knowledge == null)
			{
				return NotFound();
			}

			_context.KnowledgeOpportunities.Remove(Knowledge);
			await _context.SaveChangesAsync();

			return Knowledge;
		}

		private bool KnowledgeExists(int id)
		{
			return _context.KnowledgeOpportunities.Any(e => e.Id == id);
		}
	}
}