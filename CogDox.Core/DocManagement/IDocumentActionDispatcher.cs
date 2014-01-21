using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement
{
    public interface IDocumentActionDispatcher
    {
        object ExecuteAction(string actionName, string docRef, IDictionary<string, object> arguments);
        object ExecuteAction(string actionName, object doc, IDictionary<string, object> arguments);

    }
}
