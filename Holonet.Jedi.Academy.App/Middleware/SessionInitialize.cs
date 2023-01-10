using Microsoft.Extensions.Options;
using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public class SessionInitialize
    {
        private readonly RequestDelegate next;
        private readonly SiteConfiguration config;
        private readonly ILogger<SessionInitialize> _logger;
        //private readonly UserManager<JediAcademyAppUser> _userManager;

        public SessionInitialize(RequestDelegate next, IOptions<SiteConfiguration> options, ILogger<SessionInitialize> logger)
        {
            this.next = next;
            config = options.Value;
            _logger = logger;
            //_userManager = userManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null || context.Session == null)
            {
                return;
            }

            SessionHandler sessionHandler = new SessionHandler(context);
            UserSession? sessionObj = null;
            bool updateSession = false;
            if(!sessionHandler.ContainsActiveSession || !sessionHandler.ContainsActiveSessionUser)
            {
                if (!sessionHandler.ContainsActiveSession)
                {
                    sessionHandler.CreateSession();
                }
                sessionObj = sessionHandler.GetSession();
                if (sessionObj != null)
                {
                    SiteUser user = new SiteUser(context, config);
                    sessionObj.ActiveUser = user.CreateActiveUser();
                    await ProcessLoggedInIdentity(context, sessionObj.ActiveUser);
                    updateSession = true;
                }
            }
            if(updateSession && sessionObj != null)
            {
                sessionHandler.SaveSession(sessionObj);
            }


            await next(context);
        }

        private async Task ProcessLoggedInIdentity(HttpContext context, UserAccount user)
        {
            if(context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                JediAcademyAppUser? identityUser = await GetIdentityUserAccount(context);
                if (identityUser != null)
                {
                    PopulateSessionUserInformation(identityUser, user);
                }
            }
        }

        private async Task<JediAcademyAppUser?> GetIdentityUserAccount(HttpContext context)
        {
            JediAcademyAppUser? user = null;
            try
            {
                var _userManager = context.RequestServices.GetService<UserManager<JediAcademyAppUser>>();
                if (_userManager != null)
                {
                    user = await _userManager.GetUserAsync(context.User);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get the identity platform account from http context.");
            }
            return user;
        }

        private void PopulateSessionUserInformation(JediAcademyAppUser identityPlatformAccount, UserAccount user)
        {
            if (identityPlatformAccount != null && user != null)
            {
                user.UserId = identityPlatformAccount.UserName;
                user.Email = identityPlatformAccount.Email;
                user.FirstName = identityPlatformAccount.FirstName;
                user.LastName = identityPlatformAccount.LastName;
                user.Phone = identityPlatformAccount.PhoneNumber;
            }
        }
    }

    public static class SessionInitializeExtensions
    {
        public static IApplicationBuilder UseSessionInitialize(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionInitialize>();
        }
    }
}
