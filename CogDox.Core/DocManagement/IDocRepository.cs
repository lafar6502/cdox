using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement
{
    /// <summary>
    /// Raw document repository interface for getting documents by their type and ID
    /// </summary>
    public interface IRawDocRepository
    {
        object GetDocument(Type docType, object id);
        void ModifyDocument(Type docType, object id, Action<object> callback);
    }
}
