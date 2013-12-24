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
using CogDox.Core.Lists;
using NHibernate;

namespace CogDox.Controllers
{
    public class ListController : Controller
    {
        public IListManager ListManager { get; set; }

        public ActionResult ListData(string id, int? start, int? limit, string sort, string dir, string format = "json")
        {
            ListQuery lq = new ListQuery
            {
                Limit = limit.HasValue ? limit.Value : 20,
                Start = start.HasValue ? start.Value : 0,
                Sort = sort,
                SortAsc = "asc".Equals(dir, StringComparison.InvariantCultureIgnoreCase)
            };
            ListQueryResults res = ListManager.Query(lq, id);
            res.RawData = null;
            res.List.RecordType = null;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
