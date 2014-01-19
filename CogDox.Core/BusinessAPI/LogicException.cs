using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessAPI
{
    public class LogicException : Exception
    {
        public LogicException()
            : base()
        {
        }

        public LogicException(string msg)
            : base(msg)
        {
        }
    }
}
