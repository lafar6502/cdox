using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Web;

namespace CogDox.Core
{
    public interface ICogDoxPlugin
    {
        void Setup(ServiceConfigurator sc, HttpContext ctx);
    }
}
