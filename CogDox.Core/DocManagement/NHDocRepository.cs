using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core;
using NHibernate;

namespace CogDox.Core.DocManagement
{
    public class NHDocRepository : IRawDocRepository
    {
        public object GetDocument(Type docType, object id)
        {
            var ses = SessionContext.CurrentSession;
            return ses.Get(docType, id);
        }

        public void ModifyDocument(Type docType, object id, Action<object> callback)
        {
            var ses = SessionContext.CurrentSession;
            //bool versioned = ses.SessionFactory.GetClassMetadata(docType).IsVersioned;
            var doc = ses.Get(docType, id, LockMode.Upgrade);
            callback(doc);
            ses.Update(doc);
        }
    }
}
