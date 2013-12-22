using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.BusinessObjects;
using CogDox.Core;

namespace CogDox.ITSM.BusinessObjects
{
    public class Incident
    {
        public virtual int Id { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual UserAccount Assignee { get; set; }
        public virtual UserAccount EndUser { get; set; }
    }
}
