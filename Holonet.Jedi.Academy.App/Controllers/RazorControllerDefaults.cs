using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.BL.Session;

namespace Holonet.Jedi.Academy.App.Controllers
{
    public class RazorControllerDefaults : Controller
    {
        protected readonly SiteConfiguration Config;
        protected readonly IHttpContextAccessor CurrentHttpContext;

        public RazorControllerDefaults(IOptions<SiteConfiguration> options, IHttpContextAccessor httpContextAccessor)
        {
            Config = options.Value;
            CurrentHttpContext = httpContextAccessor;
        }

        protected bool IsSessionValid
        {
            get
            {
                SessionHandler sessionHelper = new SessionHandler(CurrentHttpContext.HttpContext);
                if (sessionHelper.ContainsActiveSession)
                {
                    UserSession? sessionObj = sessionHelper.GetSession();
                    if (sessionObj != null && !sessionObj.HasAcknowledgedConsent)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        protected UserSession? GetUserSession()
        {
            if (IsSessionValid)
            {
                SessionHandler sessionHelper = new SessionHandler(CurrentHttpContext.HttpContext);
                return sessionHelper.GetSession();
            }
            else
            {
                return null;
            }
        }

        protected UserAccount? GetActiveUser()
        {
            UserSession? session = GetUserSession();
            if (session != null)
            {
                if (session.ContainsValidUserAccount)
                {
                    return session.ActiveUser;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
