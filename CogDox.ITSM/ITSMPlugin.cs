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
            DocTypeRegistry.RegisterDocumentType<BusinessObjects.Incident>();
            
            sc.RegisterDocumentActionsFromAssembly(this.GetType().Assembly);
            sc.Modify(wc =>
            {
                NGinnBPM.MessageBus.Windsor.MessageBusConfigurator.RegisterMessageHandlersFromAssembly(this.GetType().Assembly, wc);
            });
            log.Info("ITSM plugin loaded");
        }
    }
}
