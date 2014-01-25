using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement
{
    public class DocTypeMapping : IDocTypeMapping
    {
        public string GetShortTypeName(Type t)
        {
            throw new NotImplementedException();
        }

        public Type GetDocumentType(string shortName)
        {
            throw new NotImplementedException();
        }

        public IRawDocRepository GetDocumentRepository(Type t)
        {
            throw new NotImplementedException();
        }
    }
}
