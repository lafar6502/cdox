using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Castle.Windsor;
using CogDox.Core;
using NLog;
using NHibernate;
using System.Transactions;
using CogDox.Core.Services;
using CogDox.Core.Lists;

namespace CogDox
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            SetupContainer();
            log.Info("Application started");
        }

        

        protected void Application_BeginRequest()
        {
            
        }

        protected void Application_EndRequest()
        {
            
        }

        protected void Application_PreRequestHandlerExecute()
        {
            /*log.Info("PreRequest {0}", Context.Request.Url);
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser appu = Session["_appuser"] as AppUser;
                if (appu == null)
                {
                    IWindsorContainer wc = Application["_wc"] as IWindsorContainer;
                    IAuthenticateUsers auth = wc.Resolve<IAuthenticateUsers>();
                    appu = auth.GetUserForPrincipal(Context.User);
                    Session["_appuser"] = appu;
                }
                UserContext.CurrentUser = appu;
                //System.Threading.Thread.CurrentPrincipal = appu;
            }*/
        }

        protected void Application_PostReleaseRequestState()
        {
            
        }

        protected void SetupContainer()
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
                .FinishConfiguration();
            var wc = cfg.GetContainer();
            ControllerBuilder.Current.SetControllerFactory(new Web.WindsorControllerFactory(wc));
            Web.WindsorControllerFactory.RegisterControllersFromAssembly(typeof(Controllers.HomeController).Assembly, wc);
            Application["_wc"] = wc;
            Application["_cfg"] = cfg;
            Application["_schema"] = sb.ToString();
            ListManager lm = (ListManager)wc.Resolve<IListManager>();
            foreach (var ld in ListDefs.GetListDefinitions()) lm.AddList(ld);
        }
    }
}