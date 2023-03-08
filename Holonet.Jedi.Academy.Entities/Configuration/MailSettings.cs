using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class MailSettings
	{
        public string Host { get; set; } = string.Empty;
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public bool SendAsHtml { get; set; }
		public string Sender { get; set; } = string.Empty;
		public string SenderPassword { get; set; } = string.Empty;
		public bool UseDefaultCredentials { get; set; }
	}
}
