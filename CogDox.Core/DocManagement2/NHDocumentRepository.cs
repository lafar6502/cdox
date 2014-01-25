using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;

namespace CogDox.Core.DocManagement2
{
    public class NHDocumentRepository : IDocumentRepository
    {
        public IServiceResolver Resolver { get; set; }

        protected class Config 
        {
            public Config()
            {
                ViewModelProviders = new List<IDocumentViewModelProvider>();
                ActionProviders = new List<IDocumentActionProvider>();
                EntityNames = new Dictionary<string, Type>();
            }

            public List<IDocumentViewModelProvider> ViewModelProviders { get; set; }
            public List<IDocumentActionProvider> ActionProviders { get; set; }
            public Dictionary<string, Type> EntityNames { get; set; }

            
            public void RegisterViewModelProvider(IDocumentViewModelProvider provider)
            {
                ViewModelProviders.Add(provider);
            }

            
        }

        protected Config _config = new Config();


        protected string GetDocumentVersion(object doc)
        {
            var v = SessionContext.CurrentSession.SessionFactory.GetClassMetadata(doc.GetType()).GetVersion(doc, NHibernate.EntityMode.Poco);
            if (v == null) return null;
            if (v.GetType() == typeof(byte[]))
            {
                return Convert.ToBase64String((byte[])v);
            }
            else if (v is DateTime || v is string || v is int)
            {
                return v.ToString();
            }
            return null;
        }


        public UI.DocViewModelBase GetDocumentViewModel(string docRef, string viewModelName = null)
        {
            var doc = LoadDocument(docRef);
            if (doc == null) throw new Exception("Document not found: " + docRef);
            var dt = doc.GetType();
            UI.DocViewModelBase mdl = null;
            while (dt != null)
            {
                var hnd = _config.ViewModelProviders.Find(x => x.HandlesDocumentType(dt));
                if (hnd != null)
                {
                    mdl = hnd.GetView(doc, viewModelName);
                    break;
                }
                dt = dt.BaseType;
            }
            if (mdl == null)
            {
                mdl = GetDefaultViewModel(doc);
            }
            if (string.IsNullOrEmpty(mdl.DocRef)) mdl.DocRef = docRef;
            if (string.IsNullOrEmpty(mdl.DocVersion))
            {
                mdl.DocVersion = GetDocumentVersion(doc);
            }
            foreach (var ar in Resolver.GetAllInstances<IDocumentActionRegistry>())
            {
                foreach (var aa in ar.GetActionsForType(doc.GetType()))
                {
                    if (aa.IsEnabled(doc))
                    {
                        Console.WriteLine("Action enabled: {0}", aa.ActionName);
                        var am = aa.GetModel(doc);
                        am.ParentDocRef = docRef;
                        mdl.Actions.Add(am);
                    }
                }
            }
            foreach (var ap in _config.ActionProviders)
            {
                var lst = ap.GetPossibleActions(doc, viewModelName);
                if (lst != null && lst.Any())
                {
                    mdl.Actions.AddRange(lst);
                }
            }
            return mdl;
        }

        protected UI.DocViewModelBase GetDefaultViewModel(object doc)
        {
            return new UI.DocViewModelBase
            {
                Actions = new List<UI.UIActionModel>(),
                Document = doc,
                DocRef = GetDocRef(doc),
                ViewTemplate = doc.GetType().Name
            };
        }

        public string GetDocRef(object document)
        {
            var dn = DocTypeRegistry.GetShortName(document.GetType());
            if (dn == null) throw new Exception("Document type not registered");
            return DocRef.GetEntityRef(dn, SessionContext.CurrentSessionRequired.GetIdentifier(document));
        }

        public object LoadDocument(string docRef)
        {
            var ses = SessionContext.CurrentSessionRequired;
            var en = DocRef.GetEntityName(docRef);
            var dt = DocTypeRegistry.GetDocumentType(en);
            if (dt == null) throw new Exception("Document type uknown: " + docRef);
            string sid = DocRef.GetEntityKey(docRef);
            if (string.IsNullOrEmpty(sid)) throw new Exception("Invalid document ref: " + docRef);

            var id = Convert.ChangeType(sid, ses.SessionFactory.GetClassMetadata(dt).IdentifierType.ReturnedClass);

            var doc = ses.Get(dt, id);
            return doc;
        }

        protected IDocumentAction GetAction(object doc, string actionName)
        {
            foreach (var ar in Resolver.GetAllInstances<IDocumentActionRegistry>())
            {
                var act = ar.GetAction(doc.GetType(), actionName);
                if (act != null) return act;
            }
            return null;
        }

        public object ExecuteAction(string docRef, string actionName, IDictionary<string, object> parameters, ActionOptions options)
        {
            var ses = SessionContext.CurrentSession;
            var doc = LoadDocument(docRef);
            var ver = ses.SessionFactory.GetClassMetadata(doc.GetType()).GetVersion(doc, NHibernate.EntityMode.Poco);
            var act = GetAction(doc, actionName);
            if (act == null) throw new Exception("Action not found: " + actionName);
            return act.Execute(doc, parameters);
        }
    }

    
}
