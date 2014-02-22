using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;
using System.Web.Mvc;

namespace Facile.MvcServices
{
    public class FacileDependencyResolver : IDependencyResolver
    {
        private IServiceResolver _sr; 
        public FacileDependencyResolver(IServiceResolver sr)
        {
            _sr = sr;
        }

        public object GetService(Type serviceType)
        {
            return _sr.HasService(serviceType) ? _sr.GetInstance(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _sr.GetAllInstances(serviceType);
        }
    }
}
