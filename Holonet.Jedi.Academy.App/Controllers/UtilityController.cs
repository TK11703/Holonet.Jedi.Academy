using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.BL;
using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Controllers
{
    [Route("api/utility")]
    [Produces("application/json")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    public class UtilityController : ControllerDefaults
    {
        private IMemoryCache SiteCache;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public UtilityController(IOptions<SiteConfiguration> options, IWebHostEnvironment iwebHostEnvironment, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ILogger<UtilityController> logger) 
            : base(options, httpContextAccessor)
        {
            SiteCache = memoryCache;
            _webHostEnvironment = iwebHostEnvironment;
            _logger = logger;
        }

		[Route("KeepAlive")]
		[HttpGet]
		public IActionResult KeepAlive()
		{
			try
			{
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		[Route("AcknowledgeDisclaimer")]
        [HttpGet]
        public IActionResult AcknowledgeDisclaimer()
        {
            try
            {
                JSONResponse response = new JSONResponse();
                try
                {
                    SessionHandler sessionHandler = new SessionHandler(CurrentHttpContext.HttpContext);
                    UserSession? sessionObj = sessionHandler.GetSession();
                    if (sessionObj != null)
                    {
                        
                        sessionObj.HasAcknowledgedConsent = true;
                        sessionHandler.SaveSession(sessionObj);
                        response.Success = true;
                    }
                    else
                    {
                        throw new Exception("Session object is invalid.");
                    }
                }
                catch (Exception ex)
                {
                    response.Errors.Add("Unable to register your acknowledgement of the disclaimer." + Environment.NewLine + ApplicationUtilities.GetExceptionMessages(ex, Environment.NewLine));
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString(), DateTime.UtcNow.ToLongTimeString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
