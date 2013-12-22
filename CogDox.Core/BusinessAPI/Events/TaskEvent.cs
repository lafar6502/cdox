using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI.Events
{
    public class TaskEvent : EventBase
    {
        public int TaskId { get; set; }
        public int? AssigneeGroup { get; set; }
        public int? Assignee { get; set; }
    }

    public class TaskCreated : TaskEvent
    {
    }

    public class TaskSuspended : TaskEvent
    {
    }

    public class TaskResumed : TaskEvent
    {
    }

    public class TaskAssignedToGroup : TaskEvent
    {
        public int? PrevAssigneeGroup { get; set; }
        public int? PrevAssignee { get; set; }
    }

    public class TaskAssignedToPerson : TaskEvent
    {
        public int? PrevAssignee { get; set; }
    }

    public class TaskCompleted : TaskEvent
    {
    }

    public class TaskCancelled : TaskEvent
    {
    }




    
}
