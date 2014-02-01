using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Core.Lists
{
    public class ListDef
    {
        public class ColumnDef : ListColumn
        {
            public Func<object, object> GetVal { get; set; }
        }

        public ListDef()
        {
            Columns = new List<ColumnDef>();
        }

        public string ListId { get; set; }
        public Type DaoRecordType { get; set; }
        public List<ColumnDef> Columns { get; set; }
        public Func<object, object> GetId { get; set; }
        public Func<object, Dictionary<string, string>> GetRowAttributes { get; set; }
        public Func<object, string> GetDocRef { get; set; }
        public Func<IDictionary<string, object>, ICriterion> BuildSearchFilter { get; set; }
        public Order DefaultSort { get; set; }

        public void AddColumn<T>(ColumnDef col, Func<T, object> getVal)
        {
            if (col.DataField == null) throw new Exception("Field missing");
            col.GetVal = x => getVal((T)x);
            Columns.Add(col);

            AddColumn2<BusinessObjects.GroupInfo>(gi => gi.Name, c =>
            {
                c.HeaderText = "Ala ma kota";
            });
        }

        public void AddColumn2<T>(System.Linq.Expressions.Expression<Func<T, object>> gv, Action<ColumnDef> act)
        {
            Console.WriteLine(gv.Body.ToString());
        }

        public ListDef DefineRowDocument<T>(Func<T, object> getId, Func<T, string> getDocRef, Func<T, Dictionary<string, string>> getRowAttributes)
        {
            DaoRecordType = typeof(T);
            GetId = x => getId((T)x);
            if (getRowAttributes != null) GetRowAttributes = x => getRowAttributes((T)x);
            if (getDocRef != null) GetDocRef = x => getDocRef((T)x);
            return this;
        }
    }
}
