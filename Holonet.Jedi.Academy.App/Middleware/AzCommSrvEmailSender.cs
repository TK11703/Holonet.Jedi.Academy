using Azure.Communication.Email;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Middleware
{
	public class AzCommSrvEmailSender : IEmailSender
	{
		private readonly SiteConfiguration config;
		private readonly ILogger<AzCommSrvEmailSender> _logger;

		public AzCommSrvEmailSender(IOptions<SiteConfiguration> options, ILogger<AzCommSrvEmailSender> logger)
		{
			config = options.Value;
			_logger = logger;
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			if (config.AzCommSvcSettings != null)
			{
				EmailClient emailClient = new EmailClient(config.AzCommSvcSettings.ConnectionString);
				try
				{
					Console.WriteLine("Sending email...");
					EmailSendOperation emailSendOperation = await emailClient.SendAsync(Azure.WaitUntil.Completed, config.AzCommSvcSettings.Sender, email, subject, htmlMessage);
					EmailSendResult statusMonitor = emailSendOperation.Value;

					string operationId = emailSendOperation.Id;
					var emailSendStatus = statusMonitor.Status;

					if (emailSendStatus == EmailSendStatus.Succeeded)
					{
						Console.WriteLine($"Email send operation succeeded with OperationId = {operationId}.\nEmail is out for delivery.");
					}
					else
					{
						var error = statusMonitor.Error;
						Console.WriteLine($"Failed to send email.\n OperationId = {operationId}.\n Status = {emailSendStatus}.");
						Console.WriteLine($"Error Code = {error.Code}, Message = {error.Message}");
						return;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in sending email, {ex}");
				}
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}
	}
}
