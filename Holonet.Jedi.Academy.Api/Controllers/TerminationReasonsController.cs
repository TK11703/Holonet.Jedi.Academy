using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TerminationReasonsController : ControllerBase
	{
		private readonly ILogger<TerminationReasonsController> _logger;
		private readonly AcademyContext _context;

		public TerminationReasonsController(ILogger<TerminationReasonsController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TerminationReason>>> GetTerminationReasons()
		{
			return await _context.TerminationReasons.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TerminationReason>> GetTerminationReason(int id)
		{
			var TerminationReason = await _context.TerminationReasons
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (TerminationReason == null)
			{
				return NotFound();
			}

			return TerminationReason;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTerminationReason(int id, TerminationReason item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			if (_context.TerminationReasons.Any(x => x.Name.Equals(item.Name) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.TerminationReasons.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TerminationReasonExists(id))
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
		public async Task<ActionResult<TerminationReason>> PostTerminationReason(TerminationReason item)
		{
			if (_context.TerminationReasons.Any(x => x.Name.Equals(item.Name)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.TerminationReasons.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTerminationReason", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<TerminationReason>> DeleteTerminationReason(int id)
		{
			var TerminationReason = await _context.TerminationReasons.FindAsync(id);
			if (TerminationReason == null)
			{
				return NotFound();
			}

			_context.TerminationReasons.Remove(TerminationReason);
			await _context.SaveChangesAsync();

			return TerminationReason;
		}

		private bool TerminationReasonExists(int id)
		{
			return _context.TerminationReasons.Any(e => e.Id == id);
		}
	}
}