using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Events
{
    public class EventBase
    {
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }

        public EventBase()
        {
            Timestamp = DateTime.Now;
            UserId = UserContext.CurrentUser == null ? 0 : UserContext.CurrentUser.Id;
        }
    }
}
