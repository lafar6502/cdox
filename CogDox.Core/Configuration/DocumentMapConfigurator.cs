using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Configuration
{
    public class DocumentMapConfigurator
    {
        public DocumentMapConfigurator RegisterNHDocumentType(Type docType, string docTypeName = null)
        {
            return this;
        }

        public DocumentMapConfigurator RegisterActionDef()
        {
            return this;
        }

        public DocumentMapConfigurator RegisterListDef()
        {
            return this;
        }

        public DocumentMapConfigurator SetDocumentTypeConfig(Type docType)
        {
            return this;
        }


    }
}
