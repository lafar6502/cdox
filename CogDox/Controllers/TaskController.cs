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
using CogDox.Core.BusinessObjects;

namespace CogDox.Controllers
{
    public class TaskController : CogDoxControllerBase
    {
        public ActionResult Details(int id)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = ses.Get<BaseTask>(id);
            return View(Request["embed"] != null ? "DetailsEmbed" : "Details", tsk);
        }

        public ActionResult DetailsEmbed(int id)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = ses.Get<BaseTask>(id);
            return View(tsk);
        }
    }
}
