using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}
