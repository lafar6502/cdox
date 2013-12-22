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
            ListDef ld = new ListDef
            {
                ListId = "TODO",
                DaoRecordType = typeof(BaseTask),
                GetId = x => ((BaseTask)x).Id,
                BuildSearchFilter = prm =>
                {
                    var r = Restrictions.Where<BaseTask>(x => x.TODOList && x.AssigneeGroup.IsIn(UserContext.CurrentUser.MyGroupIds) && x.Status.IsIn(new object[] { TaskStatus.Assigned, TaskStatus.Executing, TaskStatus.InGroupQueue }));
                    return r;
                }
            };
            ld.Columns.Add(new ListDef.ColumnDef
            {
                DataField = "Id",
                HeaderText = "Id",
                Sortable = true,
                GetVal = x => new HtmlString(string.Format("<a href='{1}'>{0}</a>", ((BaseTask)x).Id, Url.Action("Details", "Task", new { id = ((BaseTask) x).Id })))
            });
            ld.Columns.Add(new ListDef.ColumnDef
            {
                DataField = "CreatedDate",
                HeaderText = "CreatedDate",
                Sortable = true,
                GetVal = x => ((BaseTask)x).CreatedDate
            });
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Status",
                HeaderText = I18N.GetText("BaseTask.Status")
            }, x => I18N.GetText(x.Status));
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Summary",
                HeaderText = I18N.GetText("BaseTask.Summary"),
                Sortable = true
            }, x => x.Summary);

            ListQuery lq = new ListQuery
            {
                Start = 0,
                Limit = 20,
                QueryParameters = new Dictionary<string, object>(),
                Sort = null
            };
            var result = ListManager.Query(lq, ld);


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
