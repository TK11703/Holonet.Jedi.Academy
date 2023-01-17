using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.BL.Session
{
    public class SessionHandler
    {
        private const string SESSIONNAME = "JediAcademyApp_Session";
        private UserSession? _sessionObj;
        private HttpContext? _currentContext;

        public SessionHandler(HttpContext? context)
        {
            _currentContext = context;
            if (context != null)
            {
                _sessionObj = context.Session.Get<UserSession>(SESSIONNAME);
            }
        }

        public bool ContainsActiveSession
        {
            get
            {
                if (_sessionObj != null)
                    return true;
                else
                    return false;
            }
        }

        public bool ContainsActiveSessionUser
        {
            get
            {
                if (_sessionObj != null)
                    return _sessionObj.ContainsValidUserAccount;
                else
                    return false;
            }
        }

        public UserSession? GetSession()
        {
            return _sessionObj;
        }

        public bool CreateSession()
        {
            bool completed = false;
            _sessionObj = new UserSession();
            completed = SaveSession(_sessionObj);
            return completed;
        }

        public bool SaveSession(UserSession sessionObj)
        {
            bool completed = false;
            if (_currentContext != null)
            {
                _currentContext.Session.Set<UserSession>(SESSIONNAME, sessionObj);
                _sessionObj = _currentContext.Session.Get<UserSession>(SESSIONNAME);
                if (ContainsActiveSession)
                {
                    completed = true;
                }
            }
            return completed;
        }

        public bool ClearSession()
        {
            if (_currentContext != null)
            {
                _currentContext.Session.Clear();
            }
            return true;
        }
    }
}
