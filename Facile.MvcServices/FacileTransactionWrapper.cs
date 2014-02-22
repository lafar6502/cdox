using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web.Mvc;
using System.Web;
using Facile.Commons;
using NLog;
using System.Transactions;
using NGinnBPM.MessageBus;

namespace Facile.MvcServices
{
    public class FacileTransactionWrapper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static void OpenTransaction(ISessionFactory sf)
        {
            if (RequestContext.Current == null) throw new Exception("No RequestContext");
            var to = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(30)
            };
            var ts = new TransactionScope(TransactionScopeOption.Required, to);
            RequestContext.Current.SetItem("_transcope", ts);
            RequestContext.Current.Session = sf.OpenSession();
        }

        public static void CompleteTransaction(bool rollback)
        {            
            var rc = RequestContext.Current;
            if (rc == null) throw new Exception("Can't complete current transaction");
            var ses = rc.Session;
            if (ses == null)
            {
                log.Warn("No session in request context. ");
                return;
            }
            rc.Session = null;
            var tran = Transaction.Current;
            if (tran != null)
            {
                var ti = tran.TransactionInformation;
                if (ti.Status == TransactionStatus.Aborted) rollback = true;
                //log.Debug("Tran status: {0}", ti.Status.ToString());
            }
            else
            {
                log.Warn("No transaction");
            }
            if (ses != null)
            {
                if (!rollback)
                {
                    ses.Flush();
                }
                ses.Dispose();
            }
            var ts = rc.Get<TransactionScope>("_transcope");
            rc.SetItem("_transcope", null);
            if (ts != null)
            {
                if (!rollback)
                {
                    ts.Complete();
                }
                ts.Dispose();
            }
            else
            {
                log.Warn("no transaction scope");
            }
        }

        /// <summary>
        /// Creates RequestContext and initializes transactions as necessary.
        /// Warning: If there's RequestContext.Current it assumes someone else is managing 
        /// the transaction and doesn't start new one.
        /// </summary>
        /// <param name="act"></param>
        public static void InTransaction(ISessionFactory sf, Action act)
        {
            if (RequestContext.Current != null)
            {
                act();
                return;
            }
            bool rollback = false;
            RequestContext.Current = new RequestContext();
            try
            {
                FacileTransactionWrapper.OpenTransaction(sf);
                act();
            }
            catch(Exception)
            {
                rollback = true;
                throw;
            }
            finally
            {
                var rc = RequestContext.Current;
                FacileTransactionWrapper.CompleteTransaction(rollback || rc.RollbackTransaction);
                RequestContext.Current = null;
                if (rc != null)
                {
                    rc.Dispose();
                }
            }
        }
    }
}
