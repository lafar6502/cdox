using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using NLog;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using System.Configuration;
using NGinnBPM.MessageBus;
using NGinnBPM.MessageBus.Windsor;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using System.IO;
using CogDox.Core.Lists;
using CogDox.Core.BusinessAPI;
using CogDox.Core;
using CogDox.Core.Services;
using DM = CogDox.Core.DocManagement2;
using CogDox.Core.BusinessObjects;

namespace CogDox.Core
{
    public class ServiceConfigurator
    {
       private WindsorContainer _wc;
        private FluentConfiguration _ormConfig;
        private string _connString;
        private List<Assembly> _mappingAssemblies = new List<Assembly>();
        private List<Action<FluentConfiguration>> _ormConfigCallbacks = new List<Action<FluentConfiguration>>();
        private Logger log = LogManager.GetCurrentClassLogger();

        public static ServiceConfigurator Begin()
        {
            return new ServiceConfigurator();
        }

        protected ServiceConfigurator()
        {
            _wc = new WindsorContainer();
            log.Info("Start configuration");
        }

        public ServiceConfigurator ModifyNHConfig(Action<FluentConfiguration> act)
        {
            _ormConfigCallbacks.Add(act);
            return this;
        }

        public ServiceConfigurator AddMappingFromAssembly(Assembly asm)
        {
            _mappingAssemblies.Add(asm);
            return this;
        }

        public ServiceConfigurator Modify(Action<IWindsorContainer> act)
        {
            act(_wc);
            return this;
        }


        public ServiceConfigurator LoadPluginsFrom(Assembly asm)
        {
            var tps = asm.GetTypes().Where(t => typeof(ICogDoxPlugin).IsAssignableFrom(t)).ToList();
            foreach (var tp in tps)
            {
                if (_plugins.Any(x => x.GetType() == tp))
                {
                    log.Debug("Skipping already loaded plugin {0}", tp.Name);
                    continue;
                }
                var plugin = Activator.CreateInstance(tp) as ICogDoxPlugin;
                plugin.Setup(this, System.Web.HttpContext.Current);
                _plugins.Add(plugin);
                _wc.Register(Component.For<ICogDoxPlugin>().Instance(plugin).Named(tp.Name));
            }
            return this;
        }

        public ServiceConfigurator LoadPluginsFrom(string pluginDir)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dir = Path.IsPathRooted(pluginDir) ? pluginDir : Path.Combine(baseDir, pluginDir);
           // var resolver = new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            if (!Directory.Exists(dir)) return this;
            
            foreach (string fn in Directory.GetFiles(dir, "*.dll"))
            {
                try
                {
                    log.Debug("Trying to load plugin: {0}", fn);
                    var asm = AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(fn));
                    //Assembly asm = Assembly.Load(fn);
                    log.Debug("Loaded {0}", fn);
                    LoadPluginsFrom(asm);
                }
                catch (Exception ex)
                {
                    log.Error("Error loading {0}: {1}", fn, ex);
                }
            }
            return this;
        }

        public ServiceConfigurator UseAppSettings()
        {
            var cs = ConfigurationManager.ConnectionStrings["cogdox"];
            if (cs != null) _connString = cs.ConnectionString;
            return this;
        }

        private T[] AddResolvedToArray<T>(T[] inputArr)
        {
            if (inputArr == null) return _wc.ResolveAll<T>();
            var l = new List<T>(inputArr);
            l.AddRange(_wc.ResolveAll<T>());
            return l.ToArray();
        }
        protected ISessionFactory BuildSessionFactory()
        {
            if (string.IsNullOrEmpty(_connString)) UseAppSettings();
            _ormConfig = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(_connString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<CogDox.Core.BusinessObjects.GroupInfo>());
            foreach (var asm in _mappingAssemblies)
            {
                _ormConfig.Mappings(x => x.FluentMappings.AddFromAssembly(asm));
            }
            foreach (var act in _ormConfigCallbacks)
            {
                act(_ormConfig);
            }
            _ormConfig.ExposeConfiguration(cfg =>
            {
                cfg.EventListeners.PostLoadEventListeners = AddResolvedToArray<IPostLoadEventListener>(cfg.EventListeners.PostLoadEventListeners);
                cfg.EventListeners.PreUpdateEventListeners = AddResolvedToArray<IPreUpdateEventListener>(cfg.EventListeners.PreUpdateEventListeners);
                cfg.EventListeners.PostUpdateEventListeners = AddResolvedToArray<IPostUpdateEventListener>(cfg.EventListeners.PostUpdateEventListeners);
                cfg.EventListeners.PreInsertEventListeners = AddResolvedToArray<IPreInsertEventListener>(cfg.EventListeners.PreInsertEventListeners);
                cfg.EventListeners.PostInsertEventListeners = AddResolvedToArray<IPostInsertEventListener>(cfg.EventListeners.PostInsertEventListeners);
                cfg.EventListeners.PreDeleteEventListeners = AddResolvedToArray<IPreDeleteEventListener>(cfg.EventListeners.PreDeleteEventListeners);
                cfg.EventListeners.PostDeleteEventListeners = AddResolvedToArray<IPostDeleteEventListener>(cfg.EventListeners.PostDeleteEventListeners);
            });
            return _ormConfig.BuildSessionFactory();
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static Type ImplementsRawGeneric(Type genericInterface, Type toCheck)
        {
            foreach (var it in toCheck.GetInterfaces())
            {
                var chk = it.IsGenericType ? it.GetGenericTypeDefinition() : it;
                if (chk == genericInterface)
                {
                    return it;
                }
            }
            return null;
        }

        public static bool IsServiceRegistered(Type t, IWindsorContainer wc)
        {
            return NGinnBPM.MessageBus.Windsor.MessageBusConfigurator.IsServiceRegistered(wc, t);
        }

        

        public ServiceConfigurator RegisterDocumentActionsFromAssembly(Assembly asm)
        {
            foreach (var t in asm.GetTypes())
            {
                if (t.IsInterface || t.IsAbstract || t.IsGenericTypeDefinition) continue;
                if (typeof(DM.IDocumentAction).IsAssignableFrom(t))
                {
                    var attr = (DM.DocumentActionAttribute)Attribute.GetCustomAttribute(t, typeof(DM.DocumentActionAttribute));
                    if (attr != null && !IsServiceRegistered(t, _wc))
                    {
                        var gt = ImplementsRawGeneric(typeof(DM.IDocumentAction<>), t);
                        if (gt != null)
                        {
                            var at = gt.GetGenericArguments()[0];
                            var name = string.IsNullOrEmpty(attr.Name) ? at.Name + "." + t.Name : attr.Name;
                            log.Info("Registering action type {0} with name {1}", t.FullName, name);
                            _wc.Register(Component.For(typeof(DM.IDocumentAction), gt, t).ImplementedBy(t).LifeStyle.Transient.Named(name));
                        }
                        else
                        {
                            var name = string.IsNullOrEmpty(attr.Name) ? t.FullName : attr.Name;
                            log.Info("Registering action type {0} with name {1}", t.FullName, name);
                            _wc.Register(Component.For(typeof(DM.IDocumentAction), t).ImplementedBy(t).LifeStyle.Transient.Named(attr.Name));
                        }
                    }
                }
            }
            return this;
        }

        public ServiceConfigurator ConfigureNGinnBPM()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string pth = Path.Combine(baseDir, "..\\ProcessRepository");

            NGinnBPM.Runtime.Configuration.WindsorConfigurator.Begin(_wc)
                .UseBooProcessRepository(pth)
                .UseSqlStorage(this._connString)
                .FinishConfiguration();
            return this;
        }

        public ServiceConfigurator FinishConfiguration()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _wc.Register(Component.For<IPostLoadEventListener, IPreUpdateEventListener, IPostUpdateEventListener>()
                .Forward<IPostInsertEventListener>()
                .Forward<IPostDeleteEventListener>()
                .ImplementedBy<Services.NHEventForwarder>()
                .LifeStyle.Singleton);
            _wc.Register(Component.For<Services.IAuthenticateUsers>()
                .ImplementedBy<User.PasswordUserAuth>()
                .LifeStyle.Singleton);
            var mbc = MessageBusConfigurator.Begin(_wc)
                .ConfigureFromAppConfig()
                .AddMessageHandlersFromAssembly(typeof(ServiceConfigurator).Assembly)
                .AutoStartMessageBus(true);
            mbc.FinishConfiguration();
            _wc.Register(Component.For<ISessionFactory>().Instance(BuildSessionFactory()));
            _wc.Register(Component.For<IListManager>().ImplementedBy<ListManager>()
                .LifeStyle.Singleton);
            _wc.Register(Component.For<DM.IDocumentRepository, DM.NHDocumentRepository>()
                .ImplementedBy<DM.NHDocumentRepository>().Named("repo2").LifeStyle.Singleton);
            _wc.Register(Component.For<DM.IDocumentActionRegistry>().ImplementedBy<DM.DefaultActionRegistry>().LifeStyle.Singleton);

            _wc.Register(Component.For<ITaskOperations>().ImplementedBy<TaskOperations>().LifeStyle.Singleton);
            ConfigureApplicationDocumentRepository();
            log.Info("Finished configuration");
            return this;
        }

        protected ServiceConfigurator ConfigureApplicationDocumentRepository()
        {
            DocTypeRegistry.RegisterDocumentType<UserAccount>();
            DocTypeRegistry.RegisterDocumentType<GroupInfo>();
            DocTypeRegistry.RegisterDocumentType<BaseTask>();
            RegisterDocumentActionsFromAssembly(typeof(BusinessObjects.UserAccount).Assembly);
            return this;
        }

        public IWindsorContainer GetContainer()
        {
            return _wc;
        }

        private List<ICogDoxPlugin> _plugins = new List<ICogDoxPlugin>();

        public IEnumerable<ICogDoxPlugin> LoadedPlugins
        {
            get
            {
                return _plugins;
            }
        }

    }
}
