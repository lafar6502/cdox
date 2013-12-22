using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Messages
{
    /// <summary>
    /// create a new task instance
    /// </summary>
    public class CreateTask : TaskCommand
    {
        public string Summary { get; set; }
        public string Text { get; set; }
        public string AssigneeGroup { get; set; }
        public string Assignee { get; set; }
        public int? Category { get; set; }
    }
}
