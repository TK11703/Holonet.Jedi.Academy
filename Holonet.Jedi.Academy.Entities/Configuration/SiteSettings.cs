using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class SiteSettings
    {
        public string SessionTimeout { get; set; }
        public string SessionCookieName { get; set; }
        public bool HttpOnly { get; set; }
        public int PageSize { get; set; }
        public string CommonLongDateFormat { get; set; }
        public string CommonShortDateFormat { get; set; }
        public string GridTimestampFormat2 { get; set; }

    }
}
