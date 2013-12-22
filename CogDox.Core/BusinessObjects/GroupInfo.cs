using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class GroupInfo
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual UserAccount Supervisor { get; set; }
        public virtual bool IsProcessGroup { get; set; }
        public virtual bool IsActive { get; set; }
        /// <summary>
        /// Default deadline time for this group in minutes.
        /// </summary>
        public virtual int? DefaultDeadlineMinutes { get; set; }
        public virtual string ExtId { get; set; }
        public virtual IList<UserAccount> Members { get; set; }
        public virtual IList<UserAccount> Leaders { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
    }
}
