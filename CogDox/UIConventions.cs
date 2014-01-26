using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CogDox
{
    public class UIConventions
    {
        static Dictionary<Type, string> _defaultFieldTypes = new Dictionary<Type, string>
        {
            {typeof(string), "textfield"},
            {typeof(int), "numberfield"},
            {typeof(DateTime), "datefield"},
            {typeof(bool), "boolfield"},
            {typeof(int?), "numberfield"},
            {typeof(DateTime?), "datefield"},
            {typeof(bool?), "boolfield"}
        };

        public static string GetFieldType(Type paramType)
        {
            string s;
            return _defaultFieldTypes.TryGetValue(paramType, out s) ? s : null;
        }

        public static string GetFieldTemplate(string fieldType)
        {
            throw new NotImplementedException();
        }




    }
}