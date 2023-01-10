using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform
{
    public class IdentityPlatformSettings
    {
        public int CookieExpiresInMinutes { get; set; }
        public SigninSettings? Signin { get; set; }
        public PasswordSettings? Password { get; set; }
        public LockoutSettings? Lockout { get; set; }
        public UserSettings? User { get; set; }
        public UserSeedInformation? UserSeedInformation { get; set; }
    }
}
