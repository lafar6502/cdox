﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;


namespace CogDox.Core.BusinessAPI
{
    public interface ITaskOperations
    {
        int CreateTask(Messages.CreateTask msg);
        void CancelTask(int id, string comment);
        void TakeTaskFromGroupQueue(int id, bool startExecution = true);
        void ReturnTaskToGroupQueue(int id);
        void StartTaskExecution(int id);
        void SendTaskToGroup(int taskId, int groupId, string comment);
        void ChangeAssigneeInGroup(int taskId, int assigneeId, string comment);
        void CompleteTask(int taskId, string comment, IDictionary<string, object> completionData);
        void GetTaskDetails(int id);
        void SuspendTask(int id, DateTime unsuspendDate, string comment);
        void ResumeTask(int id, string comment);
        void PlaceTaskInCalendar(int id, string calendarId, DateTime date, int durationMinutes, string comment);
        void AddComment(int id, string comment);
        DocViewModelBase GetDetails(int id);
    }
}
