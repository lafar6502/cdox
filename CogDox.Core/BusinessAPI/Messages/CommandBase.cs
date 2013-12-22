using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Messages
{
    public class CommandBase
    {
        public string Comment { get; set; }
    }

    public class TaskCommand : CommandBase
    {
        public int TaskId { get; set; }
    }
}
