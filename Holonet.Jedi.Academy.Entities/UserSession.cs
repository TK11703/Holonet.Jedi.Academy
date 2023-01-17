using System;
using System.Linq;
using System.Collections.Generic;

namespace Holonet.Jedi.Academy.Entities
{
    public class UserSession
    {
        public UserAccount ActiveUser { get; set; }

        public bool ContainsValidUserAccount
        {
            get
            {
                if (ActiveUser != null && !string.IsNullOrEmpty(ActiveUser.UserId))
                    return true;
                else
                    return false;
            }
        }

        public bool HasAcknowledgedConsent { get; set; } = false;

        public UserSession()
        {
            ActiveUser = new UserAccount();
        }
    }
}
