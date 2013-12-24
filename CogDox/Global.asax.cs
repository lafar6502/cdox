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

        protected void OpenTransaction(HttpContext ctx)
        {
            var to = new TransactionOptions {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(30)
            };
            
            var ts = new TransactionScope(TransactionScopeOption.Required, to);
            ctx.Items.Add("_transcope", ts);
            var wc = Application["_wc"] as IWindsorContainer;
            ISessionFactory sf = wc.Resolve<ISessionFactory>();
            var ses = sf.OpenSession();
            ctx.Items.Add("_nhsession", ses);
            SessionContext.CurrentSession = ses;
        }

        protected void CompleteTransaction(HttpContext ctx)
        {
            int errors = ctx.AllErrors == null ? 0 : ctx.AllErrors.Length;
            log.Debug("CompleteTran. Errors: {0}", errors);
            var ses = SessionContext.CurrentSession;
            var rollback = errors > 0 || SessionContext.RollbackCurrentSession;
            SessionContext.CurrentSession = null;
            var tran = Transaction.Current;
            if (tran != null)
            {
                var ti = tran.TransactionInformation;
                if (ti.Status == TransactionStatus.Aborted) rollback = true;
                log.Debug("Tran status: {0}", ti.Status.ToString());
            }
            if (ses != null)
            {
                if (!rollback)
                {
                    ses.Flush();
                }
                ses.Dispose();    
            }
            var ts = ctx.Items["_transcope"] as TransactionScope;
            ctx.Items.Remove("_transcope");
            if (ts != null)
            {
                if (!rollback)
                {
                    ts.Complete();
                }
                ts.Dispose();
            }
        }

        protected void Application_BeginRequest()
        {
            log.Info("BeginRequest {0}", Context.Request.Url);
            OpenTransaction(Context);
        }

        protected void Application_EndRequest()
        {
            log.Info("EndRequest {0}", Context.Request.Url);
            CompleteTransaction(Context);
        }

        protected void Application_PreRequestHandlerExecute()
        {
            log.Info("PreRequest {0}", Context.Request.Url);
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
            }
        }

        protected void Application_PostReleaseRequestState()
        {
            log.Info("PostReleaseState {0}", Context.Request.Url);
            CompleteTransaction(Context);
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