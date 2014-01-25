using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.AppLogic.Messages
{
    public class ResumeTask
    {
        public int TaskId { get; set; }
        public bool ReturnToGroup { get; set; }
    }
}
