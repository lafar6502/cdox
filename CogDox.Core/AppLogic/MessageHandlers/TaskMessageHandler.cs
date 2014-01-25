using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;
using CogDox.Core.BusinessObjects;

namespace CogDox.Core.AppLogic.MessageHandlers
{
    public class TaskMessageHandler : IMessageConsumer<Messages.ResumeTask>
    {

        public void Handle(Messages.ResumeTask message)
        {
            var ses = SessionContext.CurrentSessionRequired;
            var tsk = ses.Get<BaseTask>(message.TaskId, NHibernate.LockMode.Write);
            throw new NotImplementedException();
            
        }
    }
}
