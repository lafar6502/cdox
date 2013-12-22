using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Messages
{
    public class SuspendTask : TaskCommand
    {
        public DateTime ResumeDate { get; set; }
    }
}
