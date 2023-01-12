using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly ILogger<StudentsController> _logger;
		private readonly AcademyContext _context;

		public StudentsController(ILogger<StudentsController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
		{
			return await _context.Students
				.Include(s => s.Species)
				.Include(s => s.Rank)
				.Include(s => s.Planet)
				.Include(s => s.ReasonForTermination)
				.Include(s => s.Quests).ThenInclude(qxp => qxp.Quest).ThenInclude(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.Include(s => s.Knowledge).ThenInclude(kxp => kxp.Knowledge)
				.Include(s => s.ForcePowers).ThenInclude(fxp => fxp.ForcePower)
				.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Student>> GetStudent(int id)
		{
			var Student = await _context.Students
				.Include(s => s.Species)
				.Include(s => s.Rank)
				.Include(s => s.Planet)
				.Include(s => s.ReasonForTermination)
				.Include(s => s.Quests).ThenInclude(qxp => qxp.Quest).ThenInclude(q => q.Objectives).ThenInclude(qo => qo.Objective).ThenInclude(o => o.Destinations).ThenInclude(d => d.Planet)
				.Include(s => s.Knowledge).ThenInclude(kxp => kxp.Knowledge)
				.Include(s => s.ForcePowers).ThenInclude(fxp => fxp.ForcePower)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (Student == null)
			{
				return NotFound();
			}

			return Student;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutStudent(int id, Student item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			_context.Students.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!StudentExists(id))
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
		public async Task<ActionResult<Student>> PostStudent(Student item)
		{
			_context.Students.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetStudent", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Student>> DeleteStudent(int id)
		{
			var Student = await _context.Students.FindAsync(id);
			if (Student == null)
			{
				return NotFound();
			}

			_context.Students.Remove(Student);
			await _context.SaveChangesAsync();

			return Student;
		}

		private bool StudentExists(int id)
		{
			return _context.Students.Any(e => e.Id == id);
		}
	}
}