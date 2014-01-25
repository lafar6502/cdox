using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;
using Castle.Windsor;

namespace CogDox.Core.DocManagement2
{
    public class DefaultActionRegistry : IDocumentActionRegistry
    {
        private IServiceResolver _resolver;

        public DefaultActionRegistry(IServiceResolver resolver)
        {
            _resolver = resolver;
        }

        public IEnumerable<IDocumentAction> GetActionsForType(Type docType)
        {
            var actsa = _resolver.GetAllInstances<IDocumentAction>();
            var tp = docType;
            string prefix = DocTypeRegistry.GetShortName(docType);
            
            _resolver.GetAllInstances<IDocumentAction>().Where(x => x.ActionName.StartsWith(prefix + "."));


            List<IDocumentAction> acts = new List<IDocumentAction>();
            foreach (var act in _resolver.GetAllInstances<IDocumentAction>())
            {
                tp = docType;
                while (tp != null)
                {
                    if (act.DocumentType == tp)
                    {
                        acts.Add(act);
                        break;
                    }
                    tp = tp.BaseType;
                }
            }
            return acts;
        }

        public IDocumentAction GetAction(Type docType, string actionName)
        {
            string prf = DocTypeRegistry.GetShortName(docType);
            if (prf == null) return null;
            prf += ".";
            var act = _resolver.GetInstance<IDocumentAction>(actionName.StartsWith(prf) ? actionName : prf + actionName);
            return act;
        }

        public static void RegisterActions()
        {
        }
    }
}
