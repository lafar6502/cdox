using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    /// <summary>
    /// History log entry
    /// </summary>
    public class ActionRecord
    {
        public ActionRecord()
        {
            TimeStamp = DateTime.Now;
        }

        public ActionRecord(object parent) : this()
        {
            var ses = SessionContext.CurrentSessionRequired;
            User = UserContext.CurrentUser != null ? UserAccount.CurrentUserAccount : null;
            ParentClass = ObjectClass.GetObjectClass(parent);
            ParentId = Convert.ToInt32(ses.GetIdentifier(parent));
        }

        public virtual long Id { get; set; }
        public virtual ObjectClass ParentClass { get; set; }
        public virtual int ParentId { get; set; }
        public virtual UserAccount User { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual ActionType Action { get; set; }
        public virtual string Summary { get; set; }
        /// <summary>
        /// Id of previous value, depentent on activity  type
        /// </summary>
        public virtual int? PrevId { get; set; }
        /// <summary>
        /// Id of new value
        /// </summary>
        public virtual int? NewId { get; set; }
        /// <summary>
        /// reference id dependent on activity type
        /// </summary>
        public virtual int? RefId { get; set; }
        /// <summary>
        /// string value, dependent on activity type
        /// </summary>
        public virtual string RefStr { get; set; }


        public static NHibernate.IQueryOver<ActionRecord, ActionRecord> GetHistory(object parent)
        {
            var ses = SessionContext.CurrentSessionRequired;
            var pc = ObjectClass.GetObjectClass(parent);
            var pid = Convert.ToInt32(ses.GetIdentifier(parent));
            return ses.QueryOver<ActionRecord>().Where(x => x.ParentClass == pc && x.ParentId == pid).OrderBy(x => x.Id).Desc;
        }

        
        
    }
}
