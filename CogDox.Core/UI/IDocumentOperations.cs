using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.UI
{
    public interface IDocumentOperations
    {
        DocDetailsModel GetDetails(string docRef);
        void ExecuteAction(string docRef, string actionName, Dictionary<string, object> arguments);

    }
}
