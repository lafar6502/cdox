using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Messages
{
    public class TaskEvent : EventBase
    {
        public int TaskId { get; set; }
    }

    public class TaskCreated : TaskEvent
    {
    }

    public class TaskCompleted : TaskEvent
    {
    }

    public class TaskCancelled : TaskEvent
    {
    }

    public class TaskStarted : TaskEvent
    {
    }

    public class TaskSuspended : TaskEvent
    {
    }

    public class TaskResumed : TaskEvent
    {
    }
}
