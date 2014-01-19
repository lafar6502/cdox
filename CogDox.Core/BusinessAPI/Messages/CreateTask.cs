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
        /// <summary>task short summary</summary>
        public string Summary { get; set; }
        /// <summary>task description html</summary>
        public string Text { get; set; }
        public string AssigneeGroup { get; set; }
        public string Assignee { get; set; }
        public int? Category { get; set; }
        public string Profile { get; set; }
        public string ExternalId { get; set; }
        public string ExternalProcessId { get; set; }
        public string ExternalTaskDef { get; set; }
        public string ExternalProcessDef { get; set; }
        public string ParentDocRef { get; set; }
        public Dictionary<string, object> TaskData { get; set; }
        /// <summary>
        /// Seconds to deadline, according to specified working time calendar
        /// </summary>
        public int? DeadlineSeconds { get; set; }
        /// <summary>
        /// Worktime calendar to use
        /// </summary>
        public string WorkTimeCalendar { get; set; }
    }
}
