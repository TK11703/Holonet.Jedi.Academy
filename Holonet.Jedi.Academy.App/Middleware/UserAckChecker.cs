using Holonet.Jedi.Academy.BL.Session;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public class UserAckChecker
    {
        private readonly RequestDelegate next;

        public UserAckChecker(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            SessionHandler sessionHandler = new SessionHandler(context);
            if (sessionHandler.ContainsActiveSession && !UserAckResource(context.Request) && ContainsAuthentictedUser(context))
            {
                UserSession? sessionObj = sessionHandler.GetSession();
                if(sessionObj != null && !sessionObj.HasAcknowledgedConsent)
                {
                    context.Response.Redirect("/Index");
                }
            }

            await next(context);
        }

        private bool ContainsAuthentictedUser(HttpContext context)
        {
            bool exists = false;
            if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                exists = true;
            }
            return exists;
        }

        private bool UserAckResource(HttpRequest request)
        {
            bool isResource = false;
            if(request.Path.Value != null && (request.Path.Value.ToLower().Equals("/index") || request.Path.Value.ToLower().Equals("/") || request.Path.Value.ToLower().Equals("/api/utility/acknowledgedodconsent")
                || request.Path.Value.ToLower().Equals("/popuptest")))
            {
                isResource = true;
            }
            return isResource;
        }
    }

    public static class UserAckCheckerExtensions
    {
        public static IApplicationBuilder UseUserAckChecker(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserAckChecker>();
        }
    }
}
