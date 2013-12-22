using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Messages
{
    public class UserSessionEvent : EventBase
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
    }

    public class UserSessionStart : UserSessionEvent
    {

    }

    public class UserSessionEnd : UserSessionEvent
    {

    }
}
