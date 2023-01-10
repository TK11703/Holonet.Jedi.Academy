using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform
{
    public class UserSeedInformation
    {
        public string UserName { get; set; } = string.Empty;

        public string DefaultPassword { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
