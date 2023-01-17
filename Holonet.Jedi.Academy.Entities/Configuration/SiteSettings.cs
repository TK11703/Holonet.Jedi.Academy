using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class SiteSettings
    {
        public string SessionTimeout { get; set; } = string.Empty;
		public string SessionCookieName { get; set; } = string.Empty;
		public bool HttpOnly { get; set; }
        public int PageSize { get; set; }
        public string CommonLongDateFormat { get; set; } = string.Empty;
		public string CommonShortDateFormat { get; set; } = string.Empty;
		public string GridTimestampFormat2 { get; set; } = string.Empty;

	}
}
