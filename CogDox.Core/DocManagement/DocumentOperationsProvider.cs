using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement
{
    /// <summary>
    /// What information do we need to collect to provide all document operations here
    /// 1. Document details
    /// 1.1 Details view model
    /// 1.2 Actions
    /// 1.3 View template
    /// 2. Document list
    /// 2.1 List definition
    /// 
    /// 1.1 How to build the view model
    /// - starting with a doc, we need to add more information (field options, etc)
    /// - we need this typed because the view will be typed with specific model type
    /// </summary>



    public class DocumentOperationsProvider : 
        IDocViewProvider,
        IDocumentActionDispatcher
    {
        private IDocTypeMapping _docTypes;
        public DocumentOperationsProvider(IDocTypeMapping dtm)
        {
            _docTypes = dtm;
        }

        public DocViewModelBase GetDetails(string docRef)
        {
            string entName = DocRef.GetEntityName(docRef);
            var dt = _docTypes.GetDocumentType(entName);
            var repo = _docTypes.GetDocumentRepository(dt);
            var doc = repo.GetDocument(dt, DocRef.GetEntityKey(docRef));
            return GetDetails(doc);
        }

        public DocViewModelBase GetDetails(object rawDoc)
        {
            List<UIActionModel> acts = new List<UIActionModel>();
            foreach(var ad in GetActionsForDocumentType(rawDoc.GetType()).OrderBy(x => x.Name))
            {
                if (ad.IsActionAllowed(rawDoc)) 
                {
                    acts.Add(ad.GetActionModel(rawDoc));
                }
            }
            return new DocViewModelBase
            {
                DocRef = DocRef.GetEntityRef(rawDoc),
                Actions = acts,
                ViewTemplate = GetViewNameFunc(rawDoc.GetType())(rawDoc),
                Document = GetViewModelBuilder(rawDoc.GetType())(rawDoc)
            };
        }

        protected IList<ActionDef> GetActionsForDocumentType(Type dt)
        {
            throw new NotImplementedException();
        }

        protected Func<object, string> GetViewNameFunc(Type docType)
        {
            throw new NotImplementedException();
        }

        protected Func<object, object> GetViewModelBuilder(Type docType)
        {
            throw new NotImplementedException();
        }

        public object ExecuteAction(string actionName, object doc, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public object ExecuteAction(string actionName, string docRef, IDictionary<string, object> arguments)
        {
            string entName = DocRef.GetEntityName(docRef);
            var dt = _docTypes.GetDocumentType(entName);
            var repo = _docTypes.GetDocumentRepository(dt);
            object ret = null;
            repo.ModifyDocument(dt, DocRef.GetEntityKey(docRef), delegate(object doc) {
                ret = ExecuteAction(actionName, doc, arguments);
            });
            return ret;
        }
    }
}
