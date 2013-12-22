using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Messages
{
    public class CompleteTask : TaskCommand
    {
        public string SolutionText { get; set; }
        public string CompletionCode { get; set; }
        public Dictionary<string, object> TaskData { get; set; }
    }
}
