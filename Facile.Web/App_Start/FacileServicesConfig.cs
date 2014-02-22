using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using NGinnBPM.MessageBus.Windsor;
using Facile.AppServices;
using Facile.Commons;
using CogDox.Core;
using NGinnBPM.MessageBus;
using System.Web.Mvc;
using System.Text;

namespace Facile.Web.App_Start
{
    public class FacileServicesConfig
    {
        public static IWindsorContainer ConfigureAppServices()
        {
            var sb = new StringBuilder();

            var cfg = ServiceConfigurator.Begin()
               .LoadPluginsFrom("Plugins")
               .ModifyNHConfig(fcfg =>
               {
                   fcfg.ExposeConfiguration(ncfg =>
                   {
                       foreach (var s in ncfg.GenerateSchemaCreationScript(new NHibernate.Dialect.MsSql2008Dialect()))
                       {
                           sb.AppendLine(s);
                       }
                   });
               })
               .ConfigureNGinnBPM()
               .Modify(wc =>
               {
                   wc.Register(Component.For<IControllerFactory>().Instance(new Facile.MvcServices.WindsorControllerFactory(wc.Kernel)));
                   Facile.MvcServices.WindsorControllerFactory.RegisterControllersFromAssembly(typeof(Controllers.HomeController).Assembly, wc);
               })
               .FinishConfiguration();
            var wct = cfg.GetContainer();
            GlobalAppData.Container = wct.Resolve<IServiceResolver>();
            return wct;
        }
    }
}