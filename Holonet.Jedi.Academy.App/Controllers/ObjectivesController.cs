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
    [Route("api/objectives")]
    [Produces("application/json")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    public class ObjectivesController : ControllerDefaults
    {
        private IMemoryCache SiteCache;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
		private readonly AcademyContext _context;

		public ObjectivesController(IOptions<SiteConfiguration> options, AcademyContext context, IWebHostEnvironment iwebHostEnvironment, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ILogger<UtilityController> logger) 
            : base(options, httpContextAccessor)
        {
            SiteCache = memoryCache;
            _webHostEnvironment = iwebHostEnvironment;
            _logger = logger;
			_context = context;
		}

        [Route("GetObjective")]
        [HttpPost]
        public async Task<IActionResult> GetObjective([FromBody] JObject data)
        {
			if (data == null || data["objectiveId"] == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Exception("Expected parameters were not found in the request."));
			}
			try
			{
				int objectiveId = data["objectiveId"].ToObject<int>();

				try
				{
					var objectiveRequested = await _context.Objectives.FindAsync(objectiveId);
					if(objectiveRequested != null && !objectiveRequested.Archived)
					{
						return Ok(objectiveRequested);
					}
					else
					{
						throw new Exception("Unable to find an active objective with that identifier.");
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Unable to retrieve the quest participation.", ex);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
    }
}
