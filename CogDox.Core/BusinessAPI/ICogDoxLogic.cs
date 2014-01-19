using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core;
using NHibernate;
using CogDox.Core.UI;

namespace CogDox.Core.BusinessAPI
{
    public interface ICogDoxLogic
    {
    }

    

    public interface IUIActionsProvider
    {
        IEnumerable<string> GetPossibleUIActions(string objref);
        object ExecuteAction(string objref, string action, Dictionary<string, object> data);
    }

    public interface IDocumentUIProvider
    {
        IEnumerable<UIActionModel> GetPossibleActions(object doc);
        object ExecuteAction(object doc, string name, Dictionary<string, object> parameters);
    }

   

    


}
