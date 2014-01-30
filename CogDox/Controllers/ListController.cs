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
using Newtonsoft.Json;
namespace CogDox.Controllers
{
    public class ListController : CogDoxControllerBase
    {
        

        public ActionResult ListData(string id, int? start, int? limit, string sort, string dir, string format = "json")
        {
            SortSpec[] sorts;
            bool asc = true;
            if (!string.IsNullOrEmpty(sort) && sort.StartsWith("["))
            {
                sorts = JsonConvert.DeserializeObject<SortSpec[]>(sort);
                sort = sorts.Length > 0 ? sorts[0].property : null;
                asc = sorts.Length > 0 ? "asc".Equals(sorts[0].direction, StringComparison.InvariantCultureIgnoreCase) : false;
            }
            
            ListQuery lq = new ListQuery
            {
                Limit = limit.HasValue ? limit.Value : 20,
                Start = start.HasValue ? start.Value : 0,
                Sort = sort,
                SortAsc = asc
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

        [Authorize]
        public ActionResult JQAjaxList(string id)
        {
            ListModel lm = ListManager.GetModel(id);
            return View(lm);
        }

        public ActionResult DTablesListData(string id, int? iDisplayStart, int? iDisplayLength, string sort, string dir, string format = "json")
        {
            sort = Request["iSortCol_0"];
            dir = Request["sSortDir_0"];
            ListQuery lq = new ListQuery
            {
                Limit = iDisplayLength.HasValue ? iDisplayLength.Value : 20,
                Start = iDisplayStart.HasValue ? iDisplayStart.Value : 0,
                Sort = sort,
                SortAsc = "asc".Equals(dir, StringComparison.InvariantCultureIgnoreCase)
            };
            ListQueryResults res = ListManager.Query(lq, id);
            Models.DataTablesQueryResults qr = new DataTablesQueryResults();
            qr.aaData = new List<IEnumerable<object>>();
            foreach (ListRow r in res.Rows)
            {
                qr.aaData.Add(r.Data);
            }
            int tot = res.Start + res.Rows.Count;
            if (res.HasMore) tot = res.Start + res.Limit + 1;
            qr.iTotalDisplayRecords = res.HasMore ? res.Start + res.Limit + 1 : res.Start + res.Rows.Count;
            qr.sEcho = Request["sEcho"] != null ? Int32.Parse(Request["sEcho"]) : 0;
            return Json(qr, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult HTList(string id, int? skip, int? take, string sort, string dir)
        {
            if (!take.HasValue || take.Value == 0) take = 20;
            var lq = new ListQuery {
                Start = skip.HasValue ? skip.Value : 0,
                Limit = take.Value,
                Sort = sort,
                SortAsc = "asc".Equals(dir, StringComparison.Ordinal)
            };
            var qr = ListManager.Query(lq, id);
            ListSearchModel lsm = new ListSearchModel
            {
                List = qr.List,
                Query = lq,
                Results = qr
            };
            return View(lsm);
        }

        [Authorize]
        public ActionResult GetListFilters(string listId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public ActionResult DeleteListFilter(string listId, string filterId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveListFilter(string listId, string filterId, string filterName)
        {
            throw new NotImplementedException();
        }
    }
}
