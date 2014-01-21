using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class ActionType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ObjectClass ParentClass { get; set; }
        public virtual string Code { get; set; }

        public virtual ActionType CreateObject
        {
            get { return SessionContext.CurrentSession.Get<ActionType>(1); }
        }

        public static ActionType FindByCode(string code)
        {
            var sc = SessionContext.CurrentSession;
            var l = sc.QueryOver<ActionType>().Where(x => x.Code == code).Take(1).List();
            return l.Count == 0 ? null : l[0];
        }

    }
}
