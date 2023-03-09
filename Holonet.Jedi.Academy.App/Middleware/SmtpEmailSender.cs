using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Holonet.Jedi.Academy.App.Middleware
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly SiteConfiguration config;
		private readonly ILogger<SmtpEmailSender> _logger;

		public SmtpEmailSender(IOptions<SiteConfiguration> options, ILogger<SmtpEmailSender> logger)
		{
			config = options.Value;
			_logger = logger;
		}
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			if (config.MailSettings != null)
			{
				SmtpClient client = new SmtpClient
				{
					Port = config.MailSettings.Port,
					Host = config.MailSettings.Host,
					EnableSsl = config.MailSettings.EnableSsl,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = config.MailSettings.UseDefaultCredentials					
				};
				if (!config.MailSettings.UseDefaultCredentials)
				{
					client.Credentials = new NetworkCredential(config.MailSettings.Sender, config.MailSettings.SenderPassword);
				}
				
				return client.SendMailAsync(config.MailSettings.Sender, email, subject, htmlMessage);
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}
	}
}
