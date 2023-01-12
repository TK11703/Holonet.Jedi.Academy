using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]	
	public class ForcePowersController : ControllerBase
	{
		private readonly ILogger<ForcePowersController> _logger;
		private readonly AcademyContext _context;

		public ForcePowersController(ILogger<ForcePowersController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ForcePower>>> GetForcePowers()
		{
			return await _context.ForcePowers.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ForcePower>> GetForcePower(int id)
		{
			var forcePower = await _context.ForcePowers
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (forcePower == null)
			{
				return NotFound();
			}

			return forcePower;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutForcePower(int id, ForcePower item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}
			if (_context.ForcePowers.Any(x => x.Name.Equals(item.Name) && !x.Id.Equals(item.Id)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));
			_context.ForcePowers.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ForcePowerExists(id))
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
		public async Task<ActionResult<ForcePower>> PostForcePower(ForcePower item)
		{
			if (_context.ForcePowers.Any(x => x.Name.Equals(item.Name)))
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("A similar item already exists."));

			_context.ForcePowers.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetForcePower", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<ForcePower>> DeleteForcePower(int id)
		{
			var ForcePower = await _context.ForcePowers.FindAsync(id);
			if (ForcePower == null)
			{
				return NotFound();
			}

			_context.ForcePowers.Remove(ForcePower);
			await _context.SaveChangesAsync();

			return ForcePower;
		}

		private bool ForcePowerExists(int id)
		{
			return _context.ForcePowers.Any(e => e.Id == id);
		}
	}
}