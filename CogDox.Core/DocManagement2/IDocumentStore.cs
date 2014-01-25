using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement2
{
    public interface IDocumentStore
    {
        object Get(Type docType, string key);
        void Modify(Type docType, string key, Action<object> modifyAction);
    }
}
