using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.DocManagement2;
using CogDox.Core.BusinessObjects;
using NHibernate;
using NGinnBPM.MessageBus;

namespace CogDox.Core.AppLogic.Task
{
    [DocumentAction]
    public class Suspend : DocumentActionBase<BaseTask>
    {
        public IMessageBus MessageBus { get; set; }

        protected override object ExecuteAction(BaseTask doc, IDictionary<string, object> parameters)
        {
            SuspendTask(doc, (DateTime)parameters["resumeDate"], (string)parameters["comment"], (bool)parameters["resumeToGroup"], (bool)parameters["putInCalendar"]);
            return null;
        }

        public void SuspendTask(BaseTask doc, DateTime resumeDate, string comment, bool resumeToGroup, bool putInCalendar)
        {
            var ses = SessionContext.CurrentSession;
            doc.Status = TaskStatus.Suspended;
            doc.ResumeDate = resumeDate;
            ActionRecord ar = new ActionRecord(doc);
            ar.Action = ActionType.FindByCode("BaseTask.Suspend");
            ar.Summary = comment;
            ses.Save(ar);
            MessageBus.Notify(new Messages.ResumeTask
            {
                ReturnToGroup = resumeToGroup,
                TaskId = doc.Id
            });
        }

        protected override bool CheckEnabled(BaseTask doc)
        {
            if (doc.Status != TaskStatus.Assigned &&
                doc.Status != TaskStatus.Executing) return false;
            if (doc.Assignee != UserAccount.CurrentUserAccount) return false;
            return true;
        }

        public override UI.UIActionModel GetModel(object doc)
        {
            var mdl = base.GetModel(doc);
            mdl.Parameters.Add(new UI.ParameterModel { Name = "resumeDate", ParamType = typeof(DateTime), FieldType = "datetime" });
            mdl.Parameters.Add(new UI.ParameterModel { Name = "resumeToGroup", ParamType = typeof(bool), FieldType = "boolean" });
            mdl.Parameters.Add(new UI.ParameterModel { Name = "putInCalendar", ParamType = typeof(bool), FieldType = "boolean" });
            mdl.Parameters.Add(new UI.ParameterModel { Name = "comment", ParamType = typeof(string), FieldType = "textarea" });

            return mdl;
        }
    }
}
