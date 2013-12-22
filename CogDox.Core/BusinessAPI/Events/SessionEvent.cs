using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Events
{
    public class SessionEvent : EventBase
    {
        public string SessionId { get; set; }
    }

    public class UserSessionStarted : SessionEvent
    {
    }

    public class UserSessionEnded : SessionEvent
    {
    }
}
