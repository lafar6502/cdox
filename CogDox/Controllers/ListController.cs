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


            List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
            foreach (var row in res.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                for (int i = 0; i < res.List.Columns.Count; i++)
                {
                    dic[res.List.Columns[i].DataField] = row.Data[i];
                }
                lst.Add(dic);
            }
            return Json(new
            {
                Data = lst,
                totalItems = 500
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
