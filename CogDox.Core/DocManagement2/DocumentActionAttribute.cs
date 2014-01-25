using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.DocManagement2
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DocumentActionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
