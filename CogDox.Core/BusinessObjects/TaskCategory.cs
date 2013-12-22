using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class TaskCategory
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Name { get; set; }
    }
}
