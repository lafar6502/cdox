using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CogDox.Models;
using CogDox.Core;
using CogDox.Core.Services;
using CogDox.Core.BusinessAPI;
using CogDox.Core.BusinessAPI.Messages;
using CogDox.Core.BusinessObjects;

namespace CogDox.Controllers
{
    public class TaskController : CogDoxControllerBase
    {
        public ITaskOperations TaskOps { get; set; }

        public ActionResult Details(int id)
        {
            return View(Request["embed"] != null ? "DetailsEmbed" : "Details", GetTaskDetailsModel(id));
        }

        public ActionResult DetailsEmbed(int id)
        {
            return View(GetTaskDetailsModel(id));
        }

        protected TaskDetailsModel GetTaskDetailsModel(int id)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = ses.Get<BaseTask>(id);
            var cb = Request["callbackjs"];
            return new TaskDetailsModel
            {
                Task = tsk,
                ViewHostCallbackJS = cb
            };
        }

        [HttpGet]
        [Authorize]
        public ActionResult NewTask()
        {
            var ses = SessionContext.CurrentSession;
            var ctvm = new CreateTaskViewModel
            {
                Profiles = ses.QueryOver<TaskProfile>().Select(x => x.Name).List<string>()
            };
            return View(ctvm);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult NewTask(CreateTask t)
        {
            var tid = TaskOps.CreateTask(t);
            return RedirectToAction("Details", new { id = tid });
        }

        public ActionResult TakeTaskFromGroupQueue(int id, bool? startExecution)
        {
            TaskOps.TakeTaskFromGroupQueue(id, startExecution.HasValue ? startExecution.Value : false);
            return Json(new object { }, JsonRequestBehavior.AllowGet);
        }
    }
}
