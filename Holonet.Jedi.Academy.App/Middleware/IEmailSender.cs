using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string htmlMessage);

		Task SendEmailAsync(List<string> emails, string subject, string htmlMessage);

		Task SendEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string htmlMessage);
	}
}
