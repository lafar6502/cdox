using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CogDox.Core.DocManagement2
{
    public class DocumentMethodAction<T> : DocumentActionBase<T>
    {
        private MethodInfo _mi;

        public DocumentMethodAction(MethodInfo mi)
        {
            _mi = mi;    
        }

        public override UI.UIActionModel GetModel(object doc)
        {
            var mdl = base.GetModel(doc);
            foreach (var pi in _mi.GetParameters())
            {
                mdl.Parameters.Add(new UI.FieldModel
                {
                    Name = pi.Name,
                    ParamType = pi.ParameterType
                });
            }
            return mdl;
        }

        protected override object ExecuteAction(T doc, IDictionary<string, object> parameters)
        {
            List<object> args = new List<object>();
            foreach(var pi in _mi.GetParameters())
            {
                if (!parameters.ContainsKey(pi.Name)) throw new Exception("Missing argument: " + pi.Name);
                args.Add(parameters[pi.Name]);
            }
            return _mi.Invoke(doc, args.ToArray());
        }

        protected override bool CheckEnabled(T doc)
        {
            throw new NotImplementedException();
        }
    }
}
