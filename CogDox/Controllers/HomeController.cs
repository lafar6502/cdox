using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CogDox.Core.Lists;
using CogDox.Core.BusinessObjects;
using CogDox.Core;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        [Authorize]
        public ActionResult AuiTest()
        {
            return View();
        }

        public ActionResult DBSchema()
        {
            return Content(Convert.ToString(this.ControllerContext.HttpContext.Application["_schema"]));
        }

        [Authorize]
        public ActionResult Fail(string id)
        {
            throw new Exception("ID: " + id);
        }

        public IListManager ListManager { get; set; }

        [Authorize]
        public ActionResult TodoList()
        {
           

            ListQuery lq = new ListQuery
            {
                Start = 0,
                Limit = 20,
                QueryParameters = new Dictionary<string, object>(),
                Sort = null
            };
            var result = ListManager.Query(lq, "TODO");


            return View("../SList/HTList", new Models.ListSearchModel
            {
                List = result.List,
                Query = lq,
                Results = result
            });
        }

        [Authorize]
        public ActionResult NewTask(string summary)
        {
            var ses = SessionContext.CurrentSession;
            var tsk = new BaseTask
            {
                ExternalId = "9898saf",
                Summary = string.IsNullOrEmpty(summary) ? " ala ma kota " : summary,
                Status = TaskStatus.InGroupQueue,
                Assignee = null,
                AssigneeGroup = UserAccount.CurrentUserAccount.MemberOf[0],
                CreatedDate = DateTime.Now,
                CurrentGroupAssignedDate = DateTime.Now,
                CurrentPersonAssignedDate = DateTime.Now,
                Description = "testowe zadanie"
            };
            ses.Save(tsk);
                
            return Content(tsk.Id.ToString());
        }

    }
}
