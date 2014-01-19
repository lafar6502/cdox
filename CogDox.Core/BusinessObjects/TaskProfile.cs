using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class TaskProfile
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        /// <summary>
        /// Users can assign this task to other group members
        /// </summary>
        public virtual bool CanUsersAssignTask { get; set; }
        /// <summary>
        /// Users can assign this task to another group
        /// </summary>
        public virtual bool CanUsersReassignToAnotherGroup { get; set; }
        /// <summary>
        /// Task can be suspended by an user
        /// </summary>
        public virtual bool CanUsersSuspendTask { get; set; }
        /// <summary>
        /// Task can be rejected by an user
        /// </summary>
        public virtual bool CanUsersRejectTask { get; set; }
        /// <summary>
        /// Task visible on TODO list
        /// </summary>
        public virtual bool ShowOnTODOList { get; set; }
        /// <summary>
        /// Task is only an action on document and does not have an independent GUI.
        /// Such task can only be completed
        /// </summary>
        public virtual bool IsDocumentAction { get; set; }

        public static TaskProfile FindByName(string name)
        {
            var l = SessionContext.CurrentSession.QueryOver<TaskProfile>().Where(x => x.Name == name).Take(1).List();
            return l.Count > 0 ? l[0] : null;
        }
    }
}
