using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement2
{
    public abstract class DocumentActionBase<T> : IDocumentAction<T>
    {
        public DocumentActionBase()
        {
            this.ActionName = typeof(T).Name + "." + this.GetType().Name;
            this.Label = I18N.GetText(this.ActionName);
            this.ShowInMenu = false;
            this.UITemplate = this.ActionName;
        }

        public virtual UI.UIActionModel GetModel(object doc)
        {
            if (!typeof(T).IsAssignableFrom(doc.GetType())) return null;
            var mdl = new UI.UIActionModel
            {
                Action = this.ActionName,
                UITemplate = this.UITemplate,
                ShowInMenu = this.ShowInMenu,
                Label = this.Label,
                Parameters = new List<UI.ParameterModel>()
            };
            return mdl;
        }

        public UI.UIActionModel GetModel(Type docType)
        {
            if (!typeof(T).IsAssignableFrom(docType)) return null;
            
            throw new NotImplementedException();
        }

        public string ActionName { get;set;}
        
        public bool IsEnabled(object doc)
        {
            if (doc == null) return false;
            if (!typeof(T).IsAssignableFrom(doc.GetType())) return false;
            return CheckEnabled((T)doc);
        }

        public object Execute(object doc, IDictionary<string, object> parameters)
        {
            return ExecuteAction((T)doc, parameters);
        }

        protected abstract object ExecuteAction(T doc, IDictionary<string, object> parameters);
        protected abstract bool CheckEnabled(T doc);

        public bool ShowInMenu { get; set; }
        public string Tooltip { get; set; }
        public string UITemplate { get; set; }
        public string Label { get; set; }



        public Type DocumentType
        {
            get { return typeof(T); }
        }
    }
}
