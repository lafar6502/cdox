using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core
{
    /// <summary>
    /// Local 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandleEvents<T>
    {
        void Handle(T message);
    }
}
