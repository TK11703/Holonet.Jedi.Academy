using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform
{
    public class UserSettings
    {
        public bool RequireUniqueEmail { get; set; }
        public string AllowedUserNameCharacters { get; set; } = string.Empty;
    }
}
