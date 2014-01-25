using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Collections.Concurrent;

namespace CogDox.Core.DocManagement
{
    public class NHDocumentManager : IDocumentManager
    {
        private class ModelProviderEntry
        {
            public Type DocType { get; set; }
            public string ViewName { get; set; }
            public Func<object, UI.DocViewModelBase> Generator { get; set; }
        }


        public NHDocumentManager()
        {
            
        }

        public bool HandlesEntityName(string name)
        {
            throw new NotImplementedException();
        }

        public UI.DocViewModelBase GetDetailsView(string docRef, string viewModelName = null)
        {
            throw new NotImplementedException();
        }

        public object ExecuteAction(string docRef, string action, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void RegisterViewModelProvider(Type docType, string viewModelName, Func<object, UI.DocViewModelBase> provider)
        {
            throw new NotImplementedException();
        }

        public void RegisterActionProvider(Type docType, IDocumentActionProvider provider)
        {
            throw new NotImplementedException();
        }
        
    }


}
