using System;
using System.Web;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Headers;

namespace Holonet.Jedi.Academy.App.Pages.Errors
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class PageErrorModel : PageModel
    {
        private readonly ILogger _logger;
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string RequestDetails { get; set; }
        public bool ShowRequestDetails => !string.IsNullOrEmpty(RequestDetails);
        public string ErrorDetails { get; set; }
        public bool ShowErrorDetails => !string.IsNullOrEmpty(ErrorDetails);

        public PageErrorModel(ILogger<PageErrorModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            RequestDetails = GetRequestDetails();
            // Do NOT expose sensitive error information directly to the client.
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ErrorDetails = GetErrorDetails(exceptionHandlerPathFeature.Error);
            return Page();
        }

        public IActionResult OnPost()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            RequestDetails = GetRequestDetails();
            // Do NOT expose sensitive error information directly to the client.
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ErrorDetails = GetErrorDetails(exceptionHandlerPathFeature.Error);
            return Page();
        }

        private string GetErrorDetails(Exception unhandledException)
        {
            StringBuilder exceptionErrors = new StringBuilder();
            exceptionErrors.AppendFormat("{0}{1}", unhandledException.Message, Environment.NewLine);
            string encodedError = HttpUtility.HtmlEncode(exceptionErrors.ToString());
            return encodedError.Replace(Environment.NewLine, "<br/>");
        }

        private string GetRequestDetails()
        {
            StringBuilder requestDetails = new StringBuilder();
            if (Request.Headers != null)
            {
                if (!string.IsNullOrEmpty(Request.Headers["AUTH_TYPE"].ToString()))
                {
                    requestDetails.AppendFormat("<strong>{0}:</strong> {1}<br/>", "AUTH_TYPE", Request.Headers["AUTH_TYPE"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Headers["User-Agent"].ToString()))
                {
                    requestDetails.AppendFormat("<strong>{0}:</strong> {1}<br/>", "User-Agent", Request.Headers["User-Agent"].ToString());
                }                
            }
            RequestHeaders header = Request.GetTypedHeaders();
            Uri uriReferer = header.Referer;
            //var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            //var url = location.AbsoluteUri;
            if (uriReferer != null && !string.IsNullOrEmpty(uriReferer.AbsoluteUri))
            {
                requestDetails.AppendFormat("<strong>{0}:</strong> {1}<br/>", "Requested Resource", uriReferer.AbsoluteUri);
            }
            return requestDetails.Replace(Environment.NewLine, "<br/>").ToString();
        }
    }
}