using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI
{

    public interface IDocumentRepository
    {
        object GetDocument(string docRef);
        string GetDocRef(object document);
        bool DoesHandleRef(string docref);
    }
}
