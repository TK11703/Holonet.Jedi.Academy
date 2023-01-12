using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SpeciesController : ControllerBase
	{
		private readonly ILogger<SpeciesController> _logger;
		private readonly AcademyContext _context;

		public SpeciesController(ILogger<SpeciesController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Species>>> GetSpecies()
		{
			return await _context.AlienRaces.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Species>> GetSpecies(int id)
		{
			var Species = await _context.AlienRaces
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (Species == null)
			{
				return NotFound();
			}

			return Species;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSpecies(int id, Species item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			if (_context.AlienRaces.Any(x => x.Name.Equals(item.Name) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.AlienRaces.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SpeciesExists(id))
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
		public async Task<ActionResult<Species>> PostSpecies(Species item)
		{
			if (_context.AlienRaces.Any(x => x.Name.Equals(item.Name)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.AlienRaces.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetSpecies", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Species>> DeleteSpecies(int id)
		{
			var Species = await _context.AlienRaces.FindAsync(id);
			if (Species == null)
			{
				return NotFound();
			}

			_context.AlienRaces.Remove(Species);
			await _context.SaveChangesAsync();

			return Species;
		}

		private bool SpeciesExists(int id)
		{
			return _context.AlienRaces.Any(e => e.Id == id);
		}
	}
}