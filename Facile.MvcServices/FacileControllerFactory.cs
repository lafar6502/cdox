using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel;
using System.Web.Mvc;
using System.Reflection;
using Castle.MicroKernel.Registration;

namespace Facile.MvcServices
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private IKernel _k;

        public WindsorControllerFactory(IKernel k)
        {
            _k = k;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;
            if (!_k.HasComponent(controllerType)) return null;
            return (IController) _k.Resolve(controllerType);
        }

        /* MVC 1.0
        protected override IController GetControllerInstance(Type controllerType)
        {
            return (IController)_wc.Resolve(controllerType);
        }*/

        public override void ReleaseController(IController controller)
        {
            _k.ReleaseComponent(controller);
        }

        public static void RegisterControllersFromAssembly(Assembly asm, IWindsorContainer wc)
        {
            foreach (Type t in asm.GetTypes())
            {
                if (typeof(IController).IsAssignableFrom(t))
                {
                    if (wc.Kernel.GetHandler(t) != null)
                    {
                        continue;
                    }
                    wc.Register(Component.For(typeof(IController), t).ImplementedBy(t).LifeStyle.Transient);
                }
            }
        }
    }
}