using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using CogDox.Core.BusinessObjects;
using NLog;
using NGinnBPM.MessageBus;
using CogDox.Core.BusinessAPI.Messages;
using NHibernate.Linq;

namespace CogDox.Core.BusinessAPI
{
    public class TaskOperations : ITaskOperations
    {
        public IMessageBus MessageBus { get; set; }

        public int CreateTask(Messages.CreateTask msg)
        {
            if (string.IsNullOrEmpty(msg.AssigneeGroup) && string.IsNullOrEmpty(msg.Assignee)) throw new LogicException("No assignee");
            if (!string.IsNullOrEmpty(msg.ExternalId))
            {
                var ot = SessionContext.CurrentSession.Query<BaseTask>().Where(x => x.ExternalId == msg.ExternalId).FirstOrDefault();
                if (ot != null) return ot.Id;
            }
            BaseTask bt = new BaseTask
            {
                Summary = msg.Summary,
                Description = msg.Text,
                TaskData = msg.TaskData,
                Status = TaskStatus.InGroupQueue,
                CreatedDate = DateTime.Now,
                CurrentGroupAssignedDate = DateTime.Now,
                CurrentPersonAssignedDate = DateTime.Now
            };
            if (!string.IsNullOrEmpty(msg.AssigneeGroup))
            {
                bt.AssigneeGroup = GroupInfo.FindGroup(msg.AssigneeGroup);
                if (bt.AssigneeGroup == null) throw new LogicException("Assignee group not found: " + msg.AssigneeGroup);
                bt.CurrentGroupAssignedDate = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(msg.Assignee))
            {
                bt.Assignee = UserAccount.FindUserByQuery(msg.Assignee, bt.AssigneeGroup);
                if (bt.Assignee == null) throw new LogicException("Assignee not found: " + msg.Assignee);
                bt.Status = TaskStatus.Assigned;
                bt.CurrentPersonAssignedDate = DateTime.Now;
            }
            string pn = string.IsNullOrEmpty(msg.Profile) ? "default" : msg.Profile;
            bt.Profile = TaskProfile.FindByName(pn);
            if (bt.Profile == null) throw new LogicException("Task profile not found: " + pn);
            bt.TODOList = bt.Profile.ShowOnTODOList;
            if (msg.DeadlineSeconds.HasValue)
            {
                bt.Deadline = DateTime.Now.AddSeconds(msg.DeadlineSeconds.Value);
            }
            SessionContext.CurrentSession.Save(bt);
            MessageBus.Notify(new Events.TaskCreated
            {
                TaskId = bt.Id,
                Assignee = bt.Assignee == null ? (int?)null : bt.Assignee.Id,
                AssigneeGroup = bt.AssigneeGroup == null ? (int?)null : bt.AssigneeGroup.Id,
                Timestamp = bt.CreatedDate,
                UserId = UserContext.CurrentUser.Id
            });
            ActionRecord ar = new ActionRecord(bt);
            ar.Action = SessionContext.CurrentSession.Get<ActionType>(1);
            ar.User = UserAccount.CurrentUserAccount;
            SessionContext.CurrentSession.Save(ar);
            return bt.Id;
        }

        public void CancelTask(int id, string comment)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = ses.Get<BaseTask>(id);
            if (tsk.Status != TaskStatus.Assigned &&
                tsk.Status != TaskStatus.InGroupQueue)
                throw new Exception("Task status invalid");

            ActionRecord ar = new ActionRecord(tsk);
            ar.Summary = comment;
            ar.Action = ses.Get<ActionType>(1);
            ses.Save(ar);
            tsk.Status = TaskStatus.Cancelled;
            tsk.CompletedDate = DateTime.Now;
            tsk.SolutionText = comment;
            ses.Update(tsk);
        }

        public void TakeTaskFromGroupQueue(int id, bool startExecution = true)
        {
            if (UserContext.CurrentUser == null) throw new Exception();
            var ses = SessionContext.CurrentSessionRequired;
            var tsk = ses.Get<BaseTask>(id);
            if (tsk.Status != TaskStatus.InGroupQueue) throw new LogicException("Invalid status");
            if (!UserAccount.CurrentUserAccount.MemberOf.Contains(tsk.AssigneeGroup)) throw new LogicException("User not in group");
            tsk.Status = startExecution ? TaskStatus.Executing : TaskStatus.Assigned;
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
            ses.Update(tsk);
            ses.Save(ar);
            MessageBus.Notify(new Events.TaskAssignedToPerson
            {
                TaskId = tsk.Id,
                Assignee = tsk.Assignee.Id,
                Timestamp = DateTime.Now,
                UserId = UserContext.CurrentUser.Id
            });
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
            ModifyTask(id, tsk =>
            {
                if (tsk.Assignee != UserAccount.CurrentUserAccount) throw new Exception("Assignee");
                if (tsk.Status != TaskStatus.Assigned && tsk.Status != TaskStatus.Executing) throw new Exception("Status");
                tsk.Status = TaskStatus.InGroupQueue;
                tsk.Assignee = null;
                tsk.CurrentPersonAssignedDate = DateTime.Now;
                var ar = new ActionRecord(tsk);
                ar.Summary = "";
                ar.Action = SessionContext.CurrentSession.Get<ActionType>(1);
                SessionContext.CurrentSession.Save(ar);
                MessageBus.Notify(new CogDox.Core.BusinessAPI.Events.TaskAssignedToGroup
                {
                    AssigneeGroup = tsk.AssigneeGroup.Id,
                    TaskId = tsk.Id,
                    Timestamp = DateTime.Now,
                    UserId = UserContext.CurrentUser.Id
                });
            });
                
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




        public void AddComment(int id, string comment)
        {
            var ses = SessionContext.CurrentSessionRequired;
            var tsk = ses.Get<BaseTask>(id);
            var ar = new ActionRecord(tsk);
            ar.Action = ActionType.FindByCode("BaseTask.AddComment");
            ar.Summary = comment;
            ses.Save(ar);
        }


        public UI.DocViewModelBase GetDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}
