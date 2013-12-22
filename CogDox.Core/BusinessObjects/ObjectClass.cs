using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace CogDox.Core.BusinessObjects
{
    public class ObjectClass
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }


        public static ObjectClass GetObjectClass(object obj)
        {
            var ses = SessionContext.CurrentSession;
            return GetObjectClass(ses.GetEntityName(obj));
        }

        private static Dictionary<string, int> _dic = null;

        public static ObjectClass GetObjectClass(string name)
        {
            var d = _dic;
            if (d == null)
            {
                d = new Dictionary<string, int>();
                var ses = SessionContext.CurrentSession;
                foreach (var oc in ses.QueryOver<ObjectClass>().List())
                {
                    d.Remove(oc.Name);
                    d[oc.Name] = oc.Id;
                }
                _dic = d; 
            }
            int id;
            return d.TryGetValue(name, out id) ? SessionContext.CurrentSession.Get<ObjectClass>(id) : null;   
        }
    }
}
