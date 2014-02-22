using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NLog;
using System.Transactions;
using NHibernate;
using CogDox.Core;
using System.Web;
using Castle.Windsor;
using Facile.Commons;
using NGinnBPM.MessageBus;

namespace Facile.MvcServices
{
    
    public class NHTransactionScopeFilter : ActionFilterAttribute, IExceptionFilter
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public IServiceResolver ServiceResolver { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            OpenTransaction(filterContext.HttpContext);
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            CompleteTransaction(filterContext.HttpContext);
        }


        public void OnException(ExceptionContext filterContext)
        {
            log.Info("OnException. {0}: {1}", filterContext.HttpContext.Request.Url, filterContext.Exception);
            CompleteTransaction(filterContext.HttpContext);
        }

        protected void OpenTransaction(HttpContextBase ctx)
        {
            log.Info("Open transaction. Url {0}", ctx.Request.Url);
            RequestContext.Current = new RequestContext();
            
            var to = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(30)
            };

            var ts = new TransactionScope(TransactionScopeOption.Required, to);
            ctx.Items.Add("_transcope", ts);
            var sf = ServiceResolver.GetInstance<ISessionFactory>();
            RequestContext.Current.Session = sf.OpenSession();
        }

        protected void CompleteTransaction(HttpContextBase ctx)
        {
            log.Info("Complete transaction. Url {0}", ctx.Request.Url);
            var rc = RequestContext.Current;
            if (rc == null) throw new Exception("Can't complete current transaction");

            int errors = ctx.AllErrors == null ? 0 : ctx.AllErrors.Length;
            log.Debug("CompleteTran. Errors: {0}", errors);
            var ses = rc.Session;
            if (ses == null)
            {
                log.Warn("No session in request context. ");
                return;
            }
            rc.Session = null;
            var rollback = errors > 0 || SessionContext.RollbackCurrentSession;
            var tran = Transaction.Current;
            if (tran != null)
            {
                var ti = tran.TransactionInformation;
                if (ti.Status == TransactionStatus.Aborted) rollback = true;
                log.Debug("Tran status: {0}", ti.Status.ToString());
            }
            else
            {
                log.Warn("No transaction in {0}", ctx.Request.Url);
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
            RequestContext.Current = null;
        }
    }
}
