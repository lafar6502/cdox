using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement2
{
    public interface IDocumentViewModelProvider
    {
        bool HandlesDocumentType(Type tdoc);
        IEnumerable<string> ViewNames { get; }
        DocViewModelBase GetView(object doc, string viewName);
    }


    public interface IDocumentActionProvider
    {
        IEnumerable<string> ActionNames { get; }
        UIActionModel GetAction(object doc, string actionName);
        IEnumerable<UIActionModel> GetPossibleActions(object doc, string viewModelName);
    }
}
