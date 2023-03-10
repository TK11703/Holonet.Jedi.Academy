using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
	public class AzCommSvcSettings
	{
		public bool Enabled { get; set; }
		public string ConnectionString { get; set; } = string.Empty;
		public string Sender { get; set; } = string.Empty;
	}
}
