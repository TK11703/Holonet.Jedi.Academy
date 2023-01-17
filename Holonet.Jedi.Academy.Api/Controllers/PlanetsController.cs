using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlanetsController : ControllerBase
	{
		private readonly ILogger<PlanetsController> _logger;
		private readonly AcademyContext _context;

		public PlanetsController(ILogger<PlanetsController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Planet>>> GetPlanets()
		{
			return await _context.Planets.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Planet>> GetPlanet(int id)
		{
			var planet = await _context.Planets
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (planet == null)
			{
				return NotFound();
			}

			return planet;
		}

		[Route("api/[controller]/[action]")]
		// GET: api/[controller]/[action]
		[HttpGet]
		public async Task<ActionResult<Planet>> GetRandom()
		{
			var planets = await _context.Planets.ToListAsync();
			if(planets == null)
			{
				return NotFound();
			}
			var random = new Random();
			int index = random.Next(planets.Count);

			return planets[index];
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutPlanet(int id, Planet item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			if (_context.Planets.Any(x => x.Name.Equals(item.Name) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.Planets.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlanetExists(id))
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
		public async Task<ActionResult<Planet>> PostPlanet(Planet item)
		{
			if (_context.Planets.Any(x => x.Name.Equals(item.Name)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.Planets.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetPlanet", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Planet>> DeletePlanet(int id)
		{
			var Planet = await _context.Planets.FindAsync(id);
			if (Planet == null)
			{
				return NotFound();
			}

			_context.Planets.Remove(Planet);
			await _context.SaveChangesAsync();

			return Planet;
		}

		private bool PlanetExists(int id)
		{
			return _context.Planets.Any(e => e.Id == id);
		}
	}
}