using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace CogDox.Core
{
    public class DocRef
    {
        public static string GetEntityRef(object entity)
        {
            throw new NotImplementedException();
        }

        public static string GetEntityRef(string entityName, object key)
        {
            throw new NotImplementedException();
        }


        public static string GetEntityName(string entityRef)
        {
            throw new NotImplementedException();
        }
        public static string GetEntityKey(string entityRef)
        {
            throw new NotImplementedException();
        }

        public static object GetEntity(string entityRef)
        {
            string ename = GetEntityName(entityRef);
            string key = GetEntityKey(entityRef);
            ISession ses = SessionContext.CurrentSession;
            return ses.Get(ename, key);
        }
    }
}
