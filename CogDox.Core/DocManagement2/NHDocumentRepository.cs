using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL = Boo.Lang;
using Rhino.DSL;

namespace CogDox.Core.DocManagement2
{
    public class NHDocumentRepository : IDocumentRepository
    {
        protected class Config : IDocumentRepositoryConfig
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

            public void RegisterActionProvider(IDocumentActionProvider actionProvider)
            {
                ActionProviders.Add(actionProvider);
            }

            public void RegisterViewModelProvider(IDocumentViewModelProvider provider)
            {
                ViewModelProviders.Add(provider);
            }

            public void RegisterDocumentType(Type tDoc, string shortEntityName = null)
            {
                if (string.IsNullOrEmpty(shortEntityName))
                {
                    shortEntityName = tDoc.Name;
                    if (EntityNames.ContainsKey(shortEntityName)) shortEntityName = tDoc.FullName;
                }
                if (EntityNames.ContainsKey(shortEntityName)) throw new Exception("Entity already registered: " + shortEntityName);
                EntityNames[shortEntityName] = tDoc;
            }
        }

        protected Config _config = new Config();

        public void UpdateConfig(Action<IDocumentRepositoryConfig> cfg)
        {
            cfg(_config);
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
            if (!_config.EntityNames.Any(x => x.Value == document.GetType())) throw new Exception("Document type not registered");
            var kv = _config.EntityNames.First(x => x.Value == document.GetType());
            return DocRef.GetEntityRef(kv.Key, SessionContext.CurrentSessionRequired.GetIdentifier(document));
        }

        public object LoadDocument(string docRef)
        {
            var ses = SessionContext.CurrentSessionRequired;
            var en = DocRef.GetEntityName(docRef);
            if (string.IsNullOrEmpty(en)) throw new Exception("Invalid document ref: " + docRef);
            Type dt;
            if (!_config.EntityNames.TryGetValue(en, out dt)) throw new Exception("Invalid entity: " + docRef);
            string sid = DocRef.GetEntityKey(docRef);
            if (string.IsNullOrEmpty(sid)) throw new Exception("Invalid document ref: " + docRef);

            var id = Convert.ChangeType(sid, ses.SessionFactory.GetClassMetadata(dt).IdentifierType.ReturnedClass);

            var doc = ses.Get(dt, id);
            return doc;
        }
    }

    public interface IDocumentRepositoryConfig
    {
        void RegisterActionProvider(IDocumentActionProvider actionProvider);
        void RegisterViewModelProvider(IDocumentViewModelProvider provider);
        void RegisterDocumentType(Type tDoc, string shortEntityName = null);
    }

    
}
