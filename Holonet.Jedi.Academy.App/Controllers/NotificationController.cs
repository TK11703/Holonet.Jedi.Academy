using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.BL;
using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities;
using Newtonsoft.Json.Linq;
using Holonet.Jedi.Academy.BL.Dashboards;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.BL.Data;

namespace Holonet.Jedi.Academy.App.Controllers
{
    [Route("api/notifications")]
    [Produces("application/json")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    public class NotificationController : ControllerDefaults
    {
        private IMemoryCache SiteCache;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
		private readonly AcademyContext _context;

		public NotificationController(IOptions<SiteConfiguration> options, AcademyContext context, IWebHostEnvironment iwebHostEnvironment, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ILogger<UtilityController> logger) 
            : base(options, httpContextAccessor)
        {
            SiteCache = memoryCache;
            _webHostEnvironment = iwebHostEnvironment;
            _logger = logger;
			_context = context;
		}

        [Route("GetNotifications")]
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
			try
			{
				UserAccount? currentUser = GetActiveUser();
				if (currentUser == null)
				{
					throw new Exception("A valid user account was not detected.");
				}

				try
				{
					var notifications = await _context.Notifications.Where(x=>x.StudentId.Equals(GetStudentId(currentUser)) && !x.Acknowledged.HasValue).ToListAsync();
					return Ok(notifications);
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the notifications.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		[Route("AckNotif")]
		[HttpPost]
		public async Task<IActionResult> AckNotif([FromBody] JObject data)
		{
			if (data == null || data["notifId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int notifId = data["notifId"].ToObject<int>();
				JSONResponse response = new JSONResponse();
				try
				{
					var notification = await _context.Notifications.FindAsync(notifId);
					if (notification != null)
					{
						notification.AcknowledgedOn = DateTime.Now;
						notification.Acknowledged = true;
						_context.Notifications.Update(notification);
						await _context.SaveChangesAsync();
						response.Success = true;
					}
					else
					{
						throw new Exception("Unable to find the requested notification with that identifier.");
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to acknowledge the notification.", ex);
				}
				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		private int GetStudentId(UserAccount currentUser)
		{
			int studentId = -1;
			var userProfileDomain = _context.UserProfiles.Where(x => x.UserId.Equals(currentUser.UserId)).FirstOrDefault();
			if (userProfileDomain != null)
			{
				studentId = userProfileDomain.StudentId;
			}
			return studentId;
		}
	}
}
