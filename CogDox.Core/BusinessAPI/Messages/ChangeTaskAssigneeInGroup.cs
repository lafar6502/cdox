using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Messages
{
    public class ChangeTaskAssigneeInGroup : TaskCommand
    {
        public int NewAssigneeId { get; set; }
    }
}
