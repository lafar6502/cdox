using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.Lists;
using CogDox.Core;
using CogDox.Core.BusinessObjects;
using NHibernate.Criterion;
using Castle.Windsor;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var wc = Configure();
        }

        static IWindsorContainer Configure()
        {
            return ServiceConfigurator.Begin()
                .AddMappingFromAssembly(typeof(CogDox.Core.BusinessObjects.UserAccount).Assembly)
                .UseAppSettings()
                .FinishConfiguration()
                .GetContainer();
        }
    }
}
