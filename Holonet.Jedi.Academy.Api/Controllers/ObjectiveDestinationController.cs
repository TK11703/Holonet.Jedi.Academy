using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ObjectiveDestinationController : ControllerBase
	{
		private readonly ILogger<ObjectiveDestinationController> _logger;
		private readonly AcademyContext _context;

		public ObjectiveDestinationController(ILogger<ObjectiveDestinationController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ObjectiveDestination>>> GetObjectiveDestinations()
		{
			return await _context.ObjectiveDestinations.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ObjectiveDestination>> GetObjectiveDestination(int id)
		{
			var ObjectiveDestination = await _context.ObjectiveDestinations
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (ObjectiveDestination == null)
			{
				return NotFound();
			}

			return ObjectiveDestination;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutObjectiveDestination(int id, ObjectiveDestination item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			if (_context.ObjectiveDestinations.Any(x => x.ObjectiveId.Equals(item.ObjectiveId) && x.PlanetId.Equals(item.PlanetId) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.ObjectiveDestinations.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ObjectiveDestinationExists(id))
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
		public async Task<ActionResult<ObjectiveDestination>> PostObjectiveDestination(ObjectiveDestination item)
		{
			if (_context.ObjectiveDestinations.Any(x => x.ObjectiveId.Equals(item.ObjectiveId) && x.PlanetId.Equals(item.PlanetId)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.ObjectiveDestinations.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetObjectiveDestination", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<ObjectiveDestination>> DeleteObjectiveDestination(int id)
		{
			var ObjectiveDestination = await _context.ObjectiveDestinations.FindAsync(id);
			if (ObjectiveDestination == null)
			{
				return NotFound();
			}

			_context.ObjectiveDestinations.Remove(ObjectiveDestination);
			await _context.SaveChangesAsync();

			return ObjectiveDestination;
		}

		private bool ObjectiveDestinationExists(int id)
		{
			return _context.ObjectiveDestinations.Any(e => e.Id == id);
		}
	}
}