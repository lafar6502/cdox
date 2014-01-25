using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement
{
    /// <summary>
    /// General document repository interface
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Check if handles specified entity name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HandlesEntityName(string name);
        /// <summary>
        /// Get details view
        /// </summary>
        /// <param name="docRef"></param>
        /// <param name="viewModelName">Name of view model, null if default view should be provided</param>
        /// <returns></returns>
        DocViewModelBase GetDetailsView(string docRef, string viewModelName = null);

        object ExecuteAction(string docRef, string action, IDictionary<string, object> arguments);
    }

    public interface IDocumentActionProvider
    {
        UIActionModel GetAction(string name, object doc);
        IEnumerable<UIActionModel> GetPossibleActions(object doc, string viewModelName);
        void ExecuteAction(object doc, string actionName, IDictionary<string, object> parameters);
    }
}
