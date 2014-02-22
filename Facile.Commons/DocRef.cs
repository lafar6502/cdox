using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Facile.Commons
{

    /// <summary>
    /// czego potrzebujemy???
    /// 
    /// ref:   [short entity name]~[entity id]
    /// 
    /// </summary>

    public class DocRef
    {
        public static string GetEntityRef(object entity)
        {
            throw new NotImplementedException();
        }

        public static string GetEntityRef(string entityName, object key)
        {
            return entityName + "~" + key.ToString();
        }


        public static string GetEntityName(string entityRef)
        {
            int idx = entityRef.IndexOf('~');
            return entityRef.Substring(0, idx < 0 ? entityRef.Length : idx);
        }

        public static string GetEntityKey(string entityRef)
        {
            int idx = entityRef.IndexOf('~');
            if (idx < 0) throw new Exception("Key not present in " + entityRef);
            return entityRef.Substring(idx + 1);
        }

        
    }
}
