using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RanksController : ControllerBase
	{
		private readonly ILogger<RanksController> _logger;
		private readonly AcademyContext _context;

		public RanksController(ILogger<RanksController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Rank>>> GetRanks()
		{
			return await _context.Ranks.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Rank>> GetRank(int id)
		{
			var rank = await _context.Ranks
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (rank == null)
			{
				return NotFound();
			}

			return rank;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutRank(int id, Rank item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			if (_context.Ranks.Any(x => x.Name.Equals(item.Name) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.Ranks.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!RankExists(id))
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
		public async Task<ActionResult<Rank>> PostRank(Rank item)
		{
			if (_context.Ranks.Any(x => x.Name.Equals(item.Name)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.Ranks.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetRank", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Rank>> DeleteRank(int id)
		{
			var Rank = await _context.Ranks.FindAsync(id);
			if (Rank == null)
			{
				return NotFound();
			}

			_context.Ranks.Remove(Rank);
			await _context.SaveChangesAsync();

			return Rank;
		}

		private bool RankExists(int id)
		{
			return _context.Ranks.Any(e => e.Id == id);
		}
	}
}