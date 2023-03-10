using FoolProof.Core;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Holonet.Jedi.Academy.App.Middleware
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly SiteConfiguration config;

		public SmtpEmailSender(SiteConfiguration siteConfig)
		{
			config = siteConfig;
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
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

				MailMessage message = GenerateMailMessage(email, subject, htmlMessage);
				await client.SendMailAsync(message);
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		public async Task SendEmailAsync(List<string> emails, string subject, string htmlMessage)
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

				MailMessage message = GenerateMailMessage(emails, subject, htmlMessage);
				await client.SendMailAsync(message);
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		public async Task SendEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage)
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

				MailMessage message = GenerateMailMessage(to, cc, bcc, subject, htmlMessage);
				await client.SendMailAsync(message);
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		private MailMessage GenerateMailMessage(string emailTo, string subject, string htmlMessage)
		{
			MailMessage message = new MailMessage(config.MailSettings.Sender, emailTo);
			message.Subject = subject;
			message.Body = htmlMessage;
			return message;
		}

		private MailMessage GenerateMailMessage(List<string> emails, string subject, string htmlMessage)
		{
			MailMessage message = new MailMessage();
			message.From = new MailAddress(config.MailSettings.Sender);
			message.Subject = subject;
			message.Body = htmlMessage;
			AssignRecipients(message, emails);
			return message;
		}

		private MailMessage GenerateMailMessage(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage)
		{
			MailMessage message = new MailMessage();
			message.From = new MailAddress(config.MailSettings.Sender);
			message.Subject = subject;
			message.Body = htmlMessage;
			AssignRecipients(message, to, cc, bcc);
			return message;
		}

		private void AssignRecipients(MailMessage message, List<string> emails)
		{
			foreach (var email in emails)
			{
				if (IsValidEmail(email))
					message.Bcc.Add(email);
			}
		}

		private void AssignRecipients(MailMessage message, List<string> to, List<string> cc, List<string> bcc)
		{
			foreach (var email in to)
			{
				if (IsValidEmail(email))
					message.To.Add(email);
			}
			foreach (var email in cc)
			{
				if (IsValidEmail(email))
					message.CC.Add(email);
			}
			foreach (var email in bcc)
			{
				if (IsValidEmail(email))
					message.Bcc.Add(email);
			}
		}

		private bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
