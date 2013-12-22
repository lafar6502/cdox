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
        public virtual bool CanUsersAssignTask { get; set; }
        public virtual bool CanUsersReassignToAnotherGroup { get; set; }
        public virtual bool CanUsersSuspendTask { get; set; }
        public virtual bool CanUsersRejectTask { get; set; }
       
    }
}
