using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement
{
    public interface IDocTypeMapping
    {
        string GetShortTypeName(Type t);
        Type GetDocumentType(string shortName);
        IRawDocRepository GetDocumentRepository(Type t);
    }
}
