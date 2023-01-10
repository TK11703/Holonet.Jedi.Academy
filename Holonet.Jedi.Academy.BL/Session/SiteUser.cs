using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Holonet.Jedi.Academy.Entities;
using Holonet.Jedi.Academy.Entities.Configuration;
using System.Linq;

namespace Holonet.Jedi.Academy.BL.Session
{
    public class SiteUser
    {
        private const string AZURE_EMAIL_HTTP_HEADER = "X-MS-CLIENT-PRINCIPAL-NAME";
        private const string AZURE_OBJECT_ID_HTTP_HEADER = "X-MS-CLIENT-PRINCIPAL-ID";
        private HttpContext _currentContext = null;
        private SiteConfiguration Config = null;

        public SiteUser(HttpContext context, SiteConfiguration config)
        {
            _currentContext = context;
            Config = config;
        }

        public UserAccount CreateActiveUser()
        {
            UserAccount activeUser = new UserAccount()
            {
                UserId = GetUserIdentifier()
            };

            return activeUser;
        }

        private string GetUserIdentifier()
        {
            string userId = string.Empty;
            userId = GetUserIdFromIdentity();
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetAzureADEmail();
            }
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetAzureADUserPrincipalId();
            }
            return userId;
        }

        private string GetUserIdFromIdentity()
        {
            string loginName = string.Empty;
            if (_currentContext != null && _currentContext.User != null && _currentContext.User.Identity != null && _currentContext.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(_currentContext.User.Identity.Name))
                {
                    if (_currentContext.User.Identity.Name.IndexOf("\\") > 0)
                    {
                        string[] loginInfo = _currentContext.User.Identity.Name.Split('\\');
                        loginName = loginInfo[loginInfo.Length - 1];
                    }
                    else
                    {
                        loginName = _currentContext.User.Identity.Name;
                    }
                }
            }
            return loginName;
        }

        private String GetAzureADEmail()
        {
            IEnumerable<string> headerValues = _currentContext.Request.Headers[AZURE_EMAIL_HTTP_HEADER];
            if (headerValues == null || headerValues.Count().Equals(0))
            {
                return string.Empty;
            }
            else
            {
                return headerValues.FirstOrDefault();
            }
        }

        private String GetAzureADUserPrincipalId()
        {
            IEnumerable<string> headerValues = _currentContext.Request.Headers[AZURE_OBJECT_ID_HTTP_HEADER];
            if (headerValues == null || headerValues.Count().Equals(0))
            {
                return string.Empty;
            }
            else
            {
                return headerValues.FirstOrDefault();
            }
        }
    }
}
