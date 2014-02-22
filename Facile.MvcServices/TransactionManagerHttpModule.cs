using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NLog;
using Facile.Commons;
using NHibernate;
using System.Diagnostics;

namespace Facile.MvcServices
{
    public class TransactionManagerHttpModule : IHttpModule
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            log.Info("Transaction manager module init");
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
            RequestContext.SetContextResolver(new Facile.MvcServices.MvcContextRequestContextResolver());
        }


        void context_EndRequest(object sender, EventArgs e)
        {
            var ct = HttpContext.Current;
            //log.Info("End request {0}. Errors: {1}", HttpContext.Current.Request.Url, HttpContext.Current.AllErrors == null ? 0 : HttpContext.Current.AllErrors.Length);
            if (RequestContext.Current == null)
            {
                log.Debug("No request context in {0}", ct.Request.Url);
                return;
            }
            var sw = Stopwatch.StartNew();
            bool rollback = RequestContext.Current.RollbackTransaction || ct.Error != null;
            try
            {
                FacileTransactionWrapper.CompleteTransaction(rollback);
            }
            catch (Exception ex)
            {
                log.Error("Error: {0}", ex);
                throw;
            }
            finally
            {
                sw.Stop();
                if (sw.ElapsedMilliseconds > 10)
                {
                    log.Info("Transaction {0} {1}. Time: {2} ms", ct.Request.Url, rollback ? "rollback" : "commit", sw.ElapsedMilliseconds);
                }
                var rc = RequestContext.Current;
                RequestContext.Current = null;
                if (rc != null)
                {
                    rc.Dispose();
                }
            }
        }


        private NHibernate.ISessionFactory _sf = null;

        void context_BeginRequest(object sender, EventArgs e)
        {
            //log.Info("Begin request {0}", HttpContext.Current.Request.Url);
            RequestContext.Current = new RequestContext();
            if (_sf == null)
            {
                _sf = GlobalAppData.Container.GetInstance<ISessionFactory>();
            }
            FacileTransactionWrapper.OpenTransaction(_sf);
        }
    }
}
