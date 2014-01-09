using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Core.Lists
{
    public class ListManager : IListManager
    {
        private Dictionary<string, ListDef> _lists = new Dictionary<string, ListDef>();

        public ISessionFactory SessionFactory { get; set; }

        public static void test()
        {
            
        }

        public void AddList(ListDef ld)
        {
            if (string.IsNullOrEmpty(ld.ListId)) throw new Exception("List ID missing");
            _lists[ld.ListId] = ld;
        }

        public ListDef GetListDef(string id)
        {
            ListDef ld;
            return _lists.TryGetValue(id, out ld) ? ld : null;
        }

        public ListModel GetModel(string listId)
        {
            var ld = GetListDef(listId);
            if (ld == null) throw new Exception("List not found: " + listId);
            return GetModel(ld);
        }

        public ListModel GetModel(ListDef ld)
        {
            ListModel lm = new ListModel
            {
                ListId = ld.ListId,
                RecordType = ld.DaoRecordType,
                Columns = new List<ListColumn>(ld.Columns.Select(c => new ListColumn {
                    HeaderText = c.HeaderText,
                    Id = c.Id,
                    DataField = c.DataField,
                    Css = c.Css,
                    Invisible = c.Invisible,
                    Hidden = c.Hidden,
                    Resizable = c.Resizable,
                    Sortable = c.Sortable,
                    Width = c.Width,
                    Flex = c.Flex,
                    DataType = c.DataType
           
                }))
            };
            return lm;
        }
        

        public ListQueryResults Query(ListQuery lq, ListDef list)
        {
            var ses = SessionContext.CurrentSessionRequired;
            if (string.IsNullOrEmpty(lq.Sort))
            {
                var md = SessionFactory.GetClassMetadata(list.DaoRecordType);
                lq.Sort = md.IdentifierPropertyName;
                lq.SortAsc = false;
            }
            int cid;
            if (Int32.TryParse(lq.Sort, out cid))
            {
                lq.Sort = list.Columns[cid].DataField;
            }
            var qf = list.BuildSearchFilter(lq.QueryParameters);
            var crit = ses.CreateCriteria(list.DaoRecordType);
            var rets = crit.Add(qf).SetFirstResult(lq.Start)
                .SetMaxResults(lq.Limit + 1).AddOrder(new Order(lq.Sort, lq.SortAsc))
                .List();
            ListQueryResults lqr = new ListQueryResults
            {
                Sort = lq.Sort,
                SortAsc = lq.SortAsc,
                HasMore = rets.Count > lq.Limit,
                Limit = lq.Limit,
                Start = lq.Start,
                RawData = new List<object>(rets.Cast<object>()),
                List = GetModel(list)
            };
            if (lqr.HasMore) lqr.RawData.RemoveAt(lqr.RawData.Count - 1);
            lqr.Rows = new List<ListRow>(lqr.RawData.Count);
            Dictionary<string, string> empty = new Dictionary<string, string>();
            foreach (var obj in lqr.RawData)
            {
                ListRow lr = new ListRow { Data = new List<object>(list.Columns.Count) };
                
                foreach (var col in list.Columns)
                {
                    if (col.GetVal != null)
                        lr.Data.Add(col.GetVal(obj));
                    else if (col.DataField != null)
                    {
                        var pi = obj.GetType().GetProperty(col.DataField);
                        if (pi == null) throw new Exception("Property not found: " + col.DataField);
                        lr.Data.Add(pi.GetValue(obj, null));
                    }
                    else throw new Exception();
                }
                lr.Attributes = empty;
                lqr.Rows.Add(lr);
            }
            return lqr;
        }

        public ListQueryResults Query(ListQuery lq, string listId)
        {
            ListDef ld;
            if (!_lists.TryGetValue(listId, out ld)) throw new Exception("List not found: " + listId);
            return Query(lq, ld);
        }
    }
}
