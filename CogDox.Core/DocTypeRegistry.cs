using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core
{
    /// <summary>
    /// Registry of document types
    /// TODO: add mapping to a repository handling particular doc type
    /// Now there's only one repo so it's unnecessary.
    /// </summary>
    public class DocTypeRegistry
    {
        private static Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public static void RegisterDocumentType<T>()
        {
            RegisterDocumentType(typeof(T));
        }

        public static void RegisterDocumentType(Type docType, string shortName = null)
        {
            if (shortName == null)
            {
                RegisterDocumentType(docType, docType.Name);
                return;
            }
            Type ot;
            if (_types.TryGetValue(shortName, out ot))
            {
                if (ot != docType) throw new Exception("Another document type registered with name " + shortName);
                return;
            }
            _types[shortName] = docType;
        }

        public static Type GetDocumentType(string shortName)
        {
            Type docType;
            return _types.TryGetValue(shortName, out docType) ? docType : null;
        }

        public static string GetShortName(Type docType)
        {
            var dt = docType;
            while (dt.BaseType != null)
            {
                var kf = _types.FirstOrDefault(x => x.Value == dt);
                if (kf.Value != null) return kf.Key;
                dt = dt.BaseType;
            }
            return null;
        }

        public static IEnumerable<string> RegisteredTypeNames
        {
            get
            {
                return _types.Keys;
            }
        }



    }
}
