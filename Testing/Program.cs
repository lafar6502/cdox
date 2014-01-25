using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.Lists;
using CogDox.Core;
using CogDox.Core.BusinessObjects;
using CogDox.Core.DocManagement2;
using NHibernate.Criterion;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using NHibernate;
using NLog;

namespace Testing
{
    class Program
    {
        static IWindsorContainer _container;
        static ISessionFactory _sessionFactory;

        static void Main(string[] args)
        {
            NLog.Config.SimpleConfigurator.ConfigureForConsoleLogging();
            _container = Configure();
            _sessionFactory = _container.Resolve<ISessionFactory>();
            using (var ses = _sessionFactory.OpenSession())
            {
                SessionContext.CurrentSession = ses;
                UserContext.CurrentUser = UserAccount.ToAppUser(ses.Get<UserAccount>(1));

                DocTypeRegistry.RegisterDocumentType<UserAccount>();
                DocTypeRegistry.RegisterDocumentType<GroupInfo>();
                DocTypeRegistry.RegisterDocumentType<BaseTask>();
                foreach (var dn in DocTypeRegistry.RegisteredTypeNames)
                {
                    Console.WriteLine("{0}=> {1}", dn, DocTypeRegistry.GetDocumentType(dn));
                }
                TestRepo1();


                ses.Flush();
            }
        }

        static IWindsorContainer Configure()
        {
            return ServiceConfigurator.Begin()
                .AddMappingFromAssembly(typeof(CogDox.Core.BusinessObjects.UserAccount).Assembly)
                .UseAppSettings()
                .Modify(wc => {
                    wc.Register(Component.For<IDocumentRepository>()
                        .ImplementedBy<NHDocumentRepository>().LifeStyle.Singleton);
                })
                .FinishConfiguration()
                .GetContainer();
        }

        static void TestRepo1()
        {
            IDocumentRepository repo = _container.Resolve<IDocumentRepository>();
            var mdl = repo.GetDocumentViewModel("BaseTask~165");
            foreach (var act in mdl.Actions)
            {
                Console.WriteLine("There's an action: {0}: {1}", act.Action, act.Label);
                foreach (var p in act.Parameters)
                {
                    Console.WriteLine("     {0}:{1}", p.Name, p.ParamType.FullName);
                }
            }
            repo.ExecuteAction(mdl.DocRef, mdl.Actions[0].Action, new Dictionary<string, object>
            {
                {"Comment", "A to taki komentarz"}
            }, new ActionOptions { DocVersion = mdl.DocVersion });
        }
    }
}
