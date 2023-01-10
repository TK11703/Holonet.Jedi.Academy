using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration
{
    public class ApplicationInformation
    {
        public string ApplicationName { get; set; } = string.Empty;
        public string ApplicationVersion { get; set; }
        public string BuildNumber { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseLabel { get; set; }
        public string SystemLabel { get; set; }
    }
}
