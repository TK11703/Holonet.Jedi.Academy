using Azure.Communication.Email;
using FoolProof.Core;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Holonet.Jedi.Academy.App.Middleware
{
	public class AzCommSrvEmailSender : IEmailSender
	{
		private readonly SiteConfiguration config;

		public AzCommSrvEmailSender(SiteConfiguration siteConfig)
		{
			config = siteConfig;
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			if (config.AzCommSvcSettings != null)
			{
				EmailClient emailClient = new EmailClient(config.AzCommSvcSettings.ConnectionString);
				try
				{
					EmailMessage message = GenerateEmailMessage(email, subject, htmlMessage);
					EmailSendOperation emailSendOperation = await emailClient.SendAsync(Azure.WaitUntil.Completed, message);
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
					throw new Exception("Email settings were not detected.", ex);
				}
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		public async Task SendEmailAsync(List<string> emails, string subject, string htmlMessage)
		{
			if (config.AzCommSvcSettings != null)
			{
				EmailClient emailClient = new EmailClient(config.AzCommSvcSettings.ConnectionString);
				try
				{
					EmailMessage message = GenerateEmailMessage(emails, subject, htmlMessage);
					EmailSendOperation emailSendOperation = await emailClient.SendAsync(Azure.WaitUntil.Completed, message);
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
					throw new Exception("Email settings were not detected.", ex);
				}
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		public async Task SendEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage)
		{
			if (config.AzCommSvcSettings != null)
			{
				EmailClient emailClient = new EmailClient(config.AzCommSvcSettings.ConnectionString);
				try
				{
					EmailMessage message = GenerateEmailMessage(to, cc, bcc, subject, htmlMessage);
					EmailSendOperation emailSendOperation = await emailClient.SendAsync(Azure.WaitUntil.Completed, message);
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
					throw new Exception("Email settings were not detected.", ex);
				}
			}
			else
			{
				throw new Exception("Email settings were not detected.");
			}
		}

		private EmailMessage GenerateEmailMessage(string emailTo, string subject, string htmlMessage)
		{
			EmailRecipients emailRecipients = GenerateRecipients(emailTo);
			EmailContent content = GenerateContents(subject, htmlMessage);
			EmailMessage message = new EmailMessage(config.AzCommSvcSettings.Sender, emailRecipients, content);
			return message;
		}

		private EmailMessage GenerateEmailMessage(List<string> emails, string subject, string htmlMessage)
		{
			EmailRecipients emailRecipients = GenerateRecipients(emails);
			EmailContent content = GenerateContents(subject, htmlMessage);
			EmailMessage message = new EmailMessage(config.AzCommSvcSettings.Sender, emailRecipients, content);
			return message;
		}

		private EmailMessage GenerateEmailMessage(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage)
		{
			EmailRecipients emailRecipients = GenerateRecipients(to, cc, bcc);
			EmailContent content = GenerateContents(subject, htmlMessage);
			EmailMessage message = new EmailMessage(config.AzCommSvcSettings.Sender, emailRecipients, content);
			return message;
		}

		private EmailRecipients GenerateRecipients(string emailTo)
		{
			var toRecipients = new List<EmailAddress>();
			if (IsValidEmail(emailTo))
				toRecipients.Add(new EmailAddress(emailTo));
			EmailRecipients emailRecipients = new EmailRecipients(toRecipients, null, null);
			return emailRecipients;
		}

		private EmailRecipients GenerateRecipients(List<string> emails)
		{
			var toRecipients = new List<EmailAddress>();
			var ccRecipients = new List<EmailAddress>();
			var bccRecipients = new List<EmailAddress>();
			foreach (var email in emails)
			{
				if (IsValidEmail(email))
					bccRecipients.Add(new EmailAddress(email));
			}
			EmailRecipients emailRecipients = new EmailRecipients(toRecipients, ccRecipients, bccRecipients);
			return emailRecipients;
		}

		private EmailRecipients GenerateRecipients(List<string> to, List<string> cc, List<string> bcc)
		{
			var toRecipients = new List<EmailAddress>();
			var ccRecipients = new List<EmailAddress>();
			var bccRecipients = new List<EmailAddress>();
			foreach (var email in to)
			{
				if (IsValidEmail(email))
					toRecipients.Add(new EmailAddress(email));
			}
			foreach (var email in cc)
			{
				if (IsValidEmail(email))
					ccRecipients.Add(new EmailAddress(email));
			}
			foreach (var email in bcc)
			{
				if (IsValidEmail(email))
					bccRecipients.Add(new EmailAddress(email));
			}

			EmailRecipients emailRecipients = new EmailRecipients(toRecipients, ccRecipients, bccRecipients);
			return emailRecipients;
		}

		private EmailContent GenerateContents(string subject, string htmlMessage)
		{
			EmailContent content = new EmailContent(subject);
			content.Html = htmlMessage;
			return content;
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
