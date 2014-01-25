using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement2
{
    public class ActionOptions
    {
        public string DocVersion { get; set; }
    }

    public interface IDocumentRepository
    {
        
        DocViewModelBase GetDocumentViewModel(string docRef, string viewModelName = null);
        object ExecuteAction(string docRef, string actionName, IDictionary<string, object> parameters, ActionOptions options = null);
    }
}
