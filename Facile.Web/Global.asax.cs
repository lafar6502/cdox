using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using NGinnBPM.MessageBus;
using NLog;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;


[assembly: PreApplicationStartMethod(typeof(Facile.Web.MvcApplication), "Initialize")]

namespace Facile.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            log.Info("Application start");
            var wc = App_Start.FacileServicesConfig.ConfigureAppServices();
            DependencyResolver.SetResolver(new Facile.MvcServices.FacileDependencyResolver(Facile.Commons.GlobalAppData.Container));
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public override void Init()
        {
            log.Info("App INIT");
            base.Init();
        }

        public static void Initialize()
        {
            log.Info("Pre-initialize modules");
            DynamicModuleUtility.RegisterModule(typeof(Facile.MvcServices.TransactionManagerHttpModule));
            log.Info("Registered transaction manager module");
        }
    }
}