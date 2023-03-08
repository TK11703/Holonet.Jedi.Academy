using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class SiteConfiguration
    {
        public const string ConfigName = "SiteConfiguration";
        public ApplicationInformation? ApplicationInformation { get; set; }
        public ContactInformation? ContactInformation { get; set; }
        public IdentityPlatformSettings? IdentityPlatform { get; set; }
        public SiteSettings? SiteSettings { get; set; }
		public MailSettings? MailSettings { get; set; }
		public DBConnections? DbConnectionStrings { get; set; }
    }
}
