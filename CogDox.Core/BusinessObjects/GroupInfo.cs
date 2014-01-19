using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

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


        public static GroupInfo FindGroup(string name)
        {
            var g = SessionContext.CurrentSession.Query<GroupInfo>().Where(x => x.IsActive && x.IsProcessGroup && (x.Name == name || x.ExtId == name)).OrderBy(x => x.Id).FirstOrDefault();
            if (g != null) return g;
            int id;
            if (Int32.TryParse(name, out id)) return SessionContext.CurrentSession.Get<GroupInfo>(id);
            return null;
        }
    }
}
