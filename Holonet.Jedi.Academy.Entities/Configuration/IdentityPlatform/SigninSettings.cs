using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform
{
    public class SigninSettings
    {
        public bool RequireConfirmedEmail { get; set; }
        public bool EnableLockouts { get; set; }
    }
}
