using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public enum TaskStatus
    {
        InGroupQueue,
        Assigned,
        Executing,
        Completed,
        Cancelled,
        Suspended
    }

    
    

    public class BaseTask
    {
        public virtual int Id { get; set; }
        public virtual int Subclass { get; set; }
        /// <summary>
        /// Task's parent class
        /// </summary>
        public virtual ObjectClass ParentClass { get; set; }
        /// <summary>
        /// Task's parent document ID
        /// </summary>
        public virtual int ParentId { get; set; }

        public virtual UserAccount Assignee { get; set; }
        public virtual GroupInfo AssigneeGroup { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Description { get; set; }
        /// <summary>
        /// Text provided as a solution/response
        /// </summary>
        public virtual string SolutionText { get; set; }
        /// <summary>
        /// Task completion code. List of completion codes is process dependent.
        /// </summary>
        public virtual string CompletionCode { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime CurrentGroupAssignedDate { get; set; }
        public virtual DateTime CurrentPersonAssignedDate { get; set; }
        /// <summary>
        /// Task deadline
        /// </summary>
        public virtual DateTime? Deadline { get; set; }
        /// <summary>
        /// Completion or cancellation date
        /// </summary>
        public virtual DateTime? CompletedDate { get; set; }
        /// <summary>
        /// Date when the task should auto-resume
        /// </summary>
        public virtual DateTime? ResumeDate { get; set; }
        /// <summary>
        /// Task profile. 
        /// </summary>
        public virtual TaskProfile Profile { get; set; }
        /// <summary>
        /// Task category. Always required.
        /// </summary>
        public virtual TaskCategory Category { get; set; }
        /// <summary>
        /// External task id (task instance id in an external process)
        /// </summary>
        public virtual string ExternalId { get; set; }
        /// <summary>
        /// Id of the external process task belongs to
        /// </summary>
        public virtual string ExternalProcessId { get; set; }
        /// <summary>
        /// External task category/task type ID
        /// </summary>
        public virtual string ExternalTaskCategoryId { get; set; }

        /// <summary>
        /// sql row version
        /// </summary>
        public virtual byte[] Version { get; set; }
        /// <summary>
        /// Should task be shown on a todo list
        /// </summary>
        public virtual bool TODOList { get; set; }
        /// <summary>
        /// Task data in JSON format
        /// </summary>
        public virtual string TaskDataJson { get; set; }
        /// <summary>
        /// Deserialized task data. Warning: the returned dictionary is 
        /// not persisted automatically. 
        /// </summary>
        public virtual Dictionary<string, object> TaskData
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }
    }
}
