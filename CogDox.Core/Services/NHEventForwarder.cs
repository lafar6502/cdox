using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using NLog;
using NGinnBPM.MessageBus;

namespace CogDox.Core.Services
{
    public class NHEventForwarder : 
        IPostUpdateEventListener,
        IPostInsertEventListener,
        IPostDeleteEventListener,
        IPreUpdateEventListener,
        IPostLoadEventListener
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        public void OnPostUpdate(PostUpdateEvent ev)
        {
            var os = string.Join(",", ev.OldState.Select(x => Convert.ToString(x)));
            var ns = string.Join(",", ev.OldState.Select(x => Convert.ToString(x)));
            log.Debug("Object updated: {0}[{1}], ({2}) => ({3})", ev.Session.GetEntityName(ev.Entity), ev.Id, os, ns);
        }

        public void OnPostInsert(PostInsertEvent ev)
        {
            log.Debug("Inserted {0}[{1}]", ev.Session.GetEntityName(ev.Entity), ev.Id);
        }

        public void OnPostDelete(PostDeleteEvent ev)
        {
            log.Debug("Deleted {0}[{1}]", ev.Session.GetEntityName(ev.Entity), ev.Id);
        }

        public bool OnPreUpdate(PreUpdateEvent ev)
        {
            log.Debug("PreUpdate {0}[{1}]", ev.Session.GetEntityName(ev.Entity), ev.Id);
            return true;
        }

        public void OnPostLoad(PostLoadEvent ev)
        {
            log.Debug("PostLoad {0}[{1}]", ev.Session.GetEntityName(ev.Entity), ev.Id);
        }
    }
}
