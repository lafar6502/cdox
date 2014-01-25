using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement2
{
    public interface IDocumentActionRegistry
    {
        IEnumerable<IDocumentAction> GetActionsForType(Type docType);
        IDocumentAction GetAction(Type docType, string actionName);
    }


    public interface IDocumentAction
    {
        /// <summary>
        /// Get action's model for specified document 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        UIActionModel GetModel(object doc);
        UIActionModel GetModel(Type docType);
        string ActionName { get; }
        Type DocumentType { get; }
        bool IsEnabled(object doc);
        
        object Execute(object doc, IDictionary<string, object> parameters);
    }

    public interface IDocumentAction<T> : IDocumentAction
    {
    }

    
}
