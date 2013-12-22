using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class Attachment
    {
        public virtual string Id { get; set; }
        public virtual long ParentId { get; set; }
        public virtual int ParentClass { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public virtual string Path { get; set; }
    }
}
