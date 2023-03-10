using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;

namespace Holonet.Jedi.Academy.App.Middleware
{
	public class CustomEmailer : IEmailSender
	{
		private readonly SiteConfiguration config;
		private readonly ILogger<CustomEmailer> _logger;

		public CustomEmailer(IOptions<SiteConfiguration> options, ILogger<CustomEmailer> logger)
		{
			config = options.Value;
			_logger = logger;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			if(config.MailSettings != null && config.MailSettings.Enabled)
			{
				SmtpEmailSender sender = new SmtpEmailSender(config);
				await sender.SendEmailAsync(email, subject, htmlMessage);
			}
			else if (config.AzCommSvcSettings != null && config.AzCommSvcSettings.Enabled)
			{
				AzCommSrvEmailSender sender = new AzCommSrvEmailSender(config);
				await sender.SendEmailAsync(email, subject, htmlMessage);
			}
			else
			{
				throw new Exception("An emailing solution was not enabled or configured.");
			}
		}

		public async Task SendEmailAsync(List<string> emails, string subject, string htmlMessage)
		{
			if (config.MailSettings != null && config.MailSettings.Enabled)
			{
				SmtpEmailSender sender = new SmtpEmailSender(config);
				await sender.SendEmailAsync(emails, subject, htmlMessage);
			}
			else if (config.AzCommSvcSettings != null && config.AzCommSvcSettings.Enabled)
			{
				AzCommSrvEmailSender sender = new AzCommSrvEmailSender(config);
				await sender.SendEmailAsync(emails, subject, htmlMessage);
			}
			else
			{
				throw new Exception("An emailing solution was not enabled or configured.");
			}
		}

		public async Task SendEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage)
		{
			if (config.MailSettings != null && config.MailSettings.Enabled)
			{
				SmtpEmailSender sender = new SmtpEmailSender(config);
				await sender.SendEmailAsync(to, cc, bcc, subject, htmlMessage);
			}
			else if (config.AzCommSvcSettings != null && config.AzCommSvcSettings.Enabled)
			{
				AzCommSrvEmailSender sender = new AzCommSrvEmailSender(config);
				await sender.SendEmailAsync(to, cc, bcc, subject, htmlMessage);
			}
			else
			{
				throw new Exception("An emailing solution was not enabled or configured.");
			}
		}
	}
}
