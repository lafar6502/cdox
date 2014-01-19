using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CogDox.Core;
using CogDox.Core.Services;
using CogDox.Core.Lists;
using System.Web.Routing;
using System.Transactions;
using NHibernate;
using NLog;
using Castle.Windsor;

namespace CogDox.Controllers
{
    public class CogDoxControllerBase : Controller
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        
        public ISessionFactory SessionFactory { get; set; }
        public IListManager ListManager { get; set; }
        public IAuthenticateUsers UserAuth { get; set; }
        public NGinnBPM.MessageBus.IServiceResolver ServiceResolver { get; set; }

        protected override void Execute(System.Web.Routing.RequestContext ctx)
        {
            bool succ = false;
            try
            {
                OpenTransaction(ctx.HttpContext);
                SessionContext.RollbackCurrentSession = true;
                if (ctx.HttpContext.User.Identity.IsAuthenticated)
                {
                    AppUser appu = ctx.HttpContext.Session["_appuser"] as AppUser;
                    if (appu == null)
                    {
                        appu = UserAuth.GetUserForPrincipal(ctx.HttpContext.User);
                        ctx.HttpContext.Session["_appuser"] = appu;
                    }
                    UserContext.CurrentUser = appu;
                }
                base.Execute(ctx);
                SessionContext.RollbackCurrentSession = false;
            }
            finally
            {
                CompleteTransaction(ctx.HttpContext);
            }
        }

        protected void OpenTransaction(HttpContextBase ctx)
        {
            var to = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(30)
            };

            var ts = new TransactionScope(TransactionScopeOption.Required, to);
            ctx.Items.Add("_transcope", ts);
            var wc = ctx.Application["_wc"] as IWindsorContainer;
            var ses = SessionFactory.OpenSession();
            ctx.Items.Add("_nhsession", ses);
            SessionContext.CurrentSession = ses;
        }

        protected void CompleteTransaction(HttpContextBase ctx)
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
    }
}