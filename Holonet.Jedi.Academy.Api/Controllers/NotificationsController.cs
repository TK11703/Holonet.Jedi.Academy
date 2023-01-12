using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holonet.Jedi.Academy.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationsController : ControllerBase
	{
		private readonly ILogger<NotificationsController> _logger;
		private readonly AcademyContext _context;

		public NotificationsController(ILogger<NotificationsController> logger, AcademyContext context)
		{
			_logger = logger;
			_context = context;
		}

		// GET: api/[controller]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
		{
			return await _context.Notifications.ToListAsync();
		}

		// GET: api/[controller]/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Notification>> GetNotification(int id)
		{
			var Notification = await _context.Notifications
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (Notification == null)
			{
				return NotFound();
			}

			return Notification;
		}

		// PUT: api/[controller]/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutNotification(int id, Notification item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

			_context.Notifications.Update(item);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!NotificationExists(id))
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
		public async Task<ActionResult<Notification>> PostNotification(Notification item)
		{
			_context.Notifications.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetNotification", new { id = item.Id }, item);
		}

		// DELETE: api/[controller]/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Notification>> DeleteNotification(int id)
		{
			var Notification = await _context.Notifications.FindAsync(id);
			if (Notification == null)
			{
				return NotFound();
			}

			_context.Notifications.Remove(Notification);
			await _context.SaveChangesAsync();

			return Notification;
		}

		private bool NotificationExists(int id)
		{
			return _context.Notifications.Any(e => e.Id == id);
		}
	}
}