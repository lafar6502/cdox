using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;
using NHibernate;
using CogDox.Core;

namespace Facile.Commons
{
    /// <summary>
    /// Represents current operation's context (like transaction context)
    /// </summary>
    public class RequestContext : IDisposable
    {
        private Dictionary<string, object> _items = new Dictionary<string, object>();

        public static RequestContext Current
        {
            get { return _cresolver.GetCurrentContext(); }
            set { _cresolver.SetCurrentContext(value); }
        }

        public object GetItem(string key)
        {
            object rv;
            return _items.TryGetValue(key, out rv) ? rv : null;
        }

        public T Get<T>(string k)
        {
            return (T)GetItem(k);
        }

        public void SetItem(string key, object item)
        {
            if (item == null)
                _items.Remove(key);
            else
                _items[key] = item;
        }

        public event Action<RequestContext> OnContextDestroy;

        public ISession Session
        {
            get { return Get<ISession>("_nhsession"); }
            set { _items["_nhsession"] = value; }
        }

        public bool RollbackTransaction
        {
            get { return Get<string>("_rolbk") != null;  }
            set { SetItem("_rolbk", value ? "yes" : null); }
        }

        
        

        public interface IContextResolver
        {
            RequestContext GetCurrentContext();
            void SetCurrentContext(RequestContext rc);
        }

        public void Dispose()
        {
            if (OnContextDestroy != null)
            {
                OnContextDestroy(this);
                OnContextDestroy = null;
            }
            
        }
        
        private static IContextResolver _cresolver = new ThreadStaticResolver();

        public static void SetContextResolver(IContextResolver cr)
        {
            _cresolver = cr;
        }


        

        public class ThreadStaticResolver : IContextResolver
        {
            [ThreadStatic]
            private static RequestContext _cur;

            public RequestContext GetCurrentContext()
            {
                return _cur;
            }

            public void SetCurrentContext(RequestContext rc)
            {
                var ctx = _cur;
                _cur = rc;
                if (ctx != null && ctx != _cur)
                {
                    ctx.Dispose();
                }
            }
        }



        
    }
}
