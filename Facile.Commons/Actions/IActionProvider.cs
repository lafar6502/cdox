using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facile.Commons.Actions
{
    public class ExecOptions
    {
    }

    public interface IActionProvider
    {
        string Name { get; }
        IEnumerable<string> GetActionNamesForDocType(Type docType);
        IEnumerable<string> GetActionsAvailableForDocument(object doc);
        ActionModel GetActionModel(string actionId);
        object ExecuteAction(object doc, string actionId, IDictionary<string, object> parameters, ExecOptions options = null);
        object ExecuteAction(object doc, string actionId, IEnumerable<object> parameters, ExecOptions options = null);
    }
}
