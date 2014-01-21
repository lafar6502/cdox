using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class WorkTimeEntry
    {
        public virtual int ActionId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual UserAccount Assignee { get; set; }
        public virtual GroupInfo Group { get; set; }
        public virtual int Duration24HSec { get; set; }
        public virtual int DurationWorkHSec { get; set; }
        public virtual UserAccount NextAssignee { get; set; }
        public virtual GroupInfo NextGroup { get; set; }
    }
}
