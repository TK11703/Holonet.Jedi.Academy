using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class ApplicationInformation
    {
        public string ApplicationName { get; set; } = string.Empty;
        public string ApplicationVersion { get; set; } = string.Empty;
		public string BuildNumber { get; set; } = string.Empty;
		public string ReleaseDate { get; set; } = string.Empty;
		public string ReleaseLabel { get; set; } = string.Empty;
		public string SystemLabel { get; set; } = string.Empty;
	}
}
