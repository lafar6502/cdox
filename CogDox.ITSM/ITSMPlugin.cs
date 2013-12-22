using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core;
using System.Web;
using NLog;

namespace CogDox.ITSM
{
    public class ITSMPlugin : ICogDoxPlugin
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public void Setup(ServiceConfigurator sc, HttpContext ctx)
        {
            log.Info("ITSM plugin loading");
            sc.AddMappingFromAssembly(this.GetType().Assembly);
        }
    }
}
