using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Linq.Expressions;

namespace CogDox.Core
{
    public class SessionContext
    {
        [ThreadStatic]
        private static ISession _ses;

        public static ISession CurrentSession
        {
            get { return _ses; }
            set 
            { 
                _ses = value; 
                _rollback = false;
            }
        }

        public static ISession CurrentSessionRequired
        {
            get
            {
                var s = CurrentSession;
                if (s == null) throw new Exception("Current session not present");
                return s;
            }
        }

        [ThreadStatic]
        private static bool _rollback = false;

        public static bool RollbackCurrentSession
        {
            get { return _rollback; }
            set { _rollback = value; }
        }
    }
}
