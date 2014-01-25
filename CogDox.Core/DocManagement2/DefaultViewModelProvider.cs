using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;


namespace CogDox.Core.DocManagement2
{

    public class DefaultViewModel<T> : DocViewModelBase
    {
        public T Doc
        {
            get { return (T)Document; }
            set { Document = value; }
        }
    }

    public class DefaultViewModelProvider<T> : IDocumentViewModelProvider
    {
        public bool HandlesDocumentType(Type tdoc)
        {
            return tdoc == typeof(T);
        }

        public IEnumerable<string> ViewNames
        {
            get { return new string[] { "default" }; }
        }

        public UI.DocViewModelBase GetView(object doc, string viewName)
        {
            return new DefaultViewModel<T>
            {
                Document = doc,
                Actions = new List<UIActionModel>(),
                ViewTemplate = null
            };
        }
    }
}
