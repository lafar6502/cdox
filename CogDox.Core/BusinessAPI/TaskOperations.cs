using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using CogDox.Core.BusinessObjects;
using NLog;
using NGinnBPM.MessageBus;
using CogDox.Core.BusinessAPI.Messages;

namespace CogDox.Core.BusinessAPI
{
    public class TaskOperations : ITaskOperations, 
        IMessageConsumer<TakeTaskFromQueue>,
        IMessageConsumer<SuspendTask>,
        IMessageHandlerService<SuspendTask>
    {
        public IMessageBus MessageBus { get; set; }

        public int CreateTask()
        {
            throw new NotImplementedException();
        }

        public void CancelTask(int id, string comment)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = ses.Get<BaseTask>(id);
            if (tsk.Status != TaskStatus.Assigned &&
                tsk.Status != TaskStatus.InGroupQueue)
                throw new Exception("Task status invalid");
            
            throw new NotImplementedException();
        }

        public void TakeTaskFromGroupQueue(int id, bool startExecution = true)
        {
            if (UserContext.CurrentUser == null) throw new Exception();
            var ses = SessionContext.CurrentSessionRequired;
            var tsk = ses.Get<BaseTask>(id);
            if (tsk.Status != TaskStatus.InGroupQueue) throw new Exception("Invalid status");
            if (!UserAccount.CurrentUserAccount.MemberOf.Contains(tsk.AssigneeGroup))
                throw new Exception("User not in group");
            tsk.Status = TaskStatus.Assigned;
            tsk.CurrentPersonAssignedDate = DateTime.Now;
            var oldAss = tsk.Assignee;
            tsk.Assignee = UserAccount.CurrentUserAccount;
            var ar = new ActionRecord {
                ParentClass = ObjectClass.GetObjectClass(tsk),
                ParentId = tsk.Id,
                Action = ses.Get<ActionType>(1),
                Summary = "",
                NewId = tsk.Assignee.Id,
                PrevId = oldAss != null ? oldAss.Id : (int?) null,
                TimeStamp = DateTime.Now,
                User = UserAccount.CurrentUserAccount
            };
            ses.Save(ar);
        }

        protected void ModifyTask(int id, Action<BaseTask> act)
        {
            if (UserContext.CurrentUser == null) throw new Exception();
            var ses = SessionContext.CurrentSessionRequired;
            var tsk = ses.Get<BaseTask>(id);
            act(tsk);
            ses.Update(tsk);
        }

        public void ReturnTaskToGroupQueue(int id)
        {
            throw new NotImplementedException();
        }

        public void StartTaskExecution(int id)
        {
            throw new NotImplementedException();
        }

        public void SendTaskToGroup(int taskId, int groupId, string comment)
        {
            throw new NotImplementedException();
        }

        public void ChangeAssigneeInGroup(int taskId, int assigneeId, string comment)
        {
            throw new NotImplementedException();
        }

        public void CompleteTask(int taskId, string comment, IDictionary<string, object> completionData)
        {
            throw new NotImplementedException();
        }

        public void GetTaskDetails(int id)
        {
            throw new NotImplementedException();
        }

        public void SuspendTask(int id, DateTime unsuspendDate, string comment)
        {
            Handle(new SuspendTask { Comment = comment, TaskId = id, ResumeDate = unsuspendDate });
        }

        public void ResumeTask(int id, string comment)
        {
            throw new NotImplementedException();
        }

        public void PlaceTaskInCalendar(int id, string calendarId, DateTime date, int durationMinutes, string comment)
        {
            throw new NotImplementedException();
        }

        public void Handle(TakeTaskFromQueue message)
        {
            var ses = SessionContext.CurrentSessionRequired;
            this.TakeTaskFromGroupQueue(message.TaskId, message.StartExecution);
        }

        public void Handle(SuspendTask message)
        {
            var ses = SessionContext.CurrentSessionRequired;
            ModifyTask(message.TaskId, t =>
            {
                if (t.Status == TaskStatus.Suspended)
                {
                    return;
                }
                if (t.Status == TaskStatus.InGroupQueue)
                {
                }
                else if (t.Status == TaskStatus.Assigned || t.Status == TaskStatus.Executing)
                {
                    if (t.Assignee != UserAccount.CurrentUserAccount) throw new Exception("Invalid user");
                }
                t.Status = TaskStatus.Suspended;
                t.ResumeDate = message.ResumeDate;
                var ar = new ActionRecord(this)
                {
                    Summary = message.Comment
                };
                ses.Save(ar);
            });
        }

        object IMessageHandlerService<SuspendTask>.Handle(SuspendTask message)
        {
            Handle(message);
            return null;
        }
    }
}
