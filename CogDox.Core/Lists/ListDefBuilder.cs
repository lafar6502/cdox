using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Lists
{
    public class ListDefBuilder<T>
    {
        private List<ListDef.ColumnDef> _cols = new List<ListDef.ColumnDef>();
        private Func<T, object> _getId;
        private Func<T, string> _getDocRef;
        private Action<T, Dictionary<string, string>> _rowAttrs;
        private ListDef _curDef = new ListDef();

        public ListDefBuilder()
        {
            _curDef.ListId = typeof(T).Name;
        }

        public ListDefBuilder(string listId)
        {
            _curDef.ListId = listId;
        }


        public class ColumnDefBuilder<T>
        {
            private ListDef.ColumnDef _col = new ListDef.ColumnDef();

            public ColumnDefBuilder(string name)
            {
                _col.Id = name;
                _col.HeaderText = name;
                _col.DataField = name;
            }

            public ColumnDefBuilder<T> Val(Func<T, object> getval)
            {
                _col.GetVal = x => getval((T)x);
                return this;
            }

            public ColumnDefBuilder<T> Sortable(bool b)
            {
                _col.Sortable = b;
                return this;
            }

            public ColumnDefBuilder<T> Hidden(bool b = true)
            {
                _col.Hidden = b;
                return this;
            }

            public ColumnDefBuilder<T> CssClass(string cls)
            {
                _col.Css = cls;
                return this;
            }

            public ColumnDefBuilder<T> HeaderText(string txt)
            {
                _col.HeaderText = txt;
                return this;
            }

            public ColumnDefBuilder<T> DataType(string typeName)
            {
                _col.DataType = typeName;
                return this;
            }

            public ColumnDefBuilder<T> DataField(string name)
            {
                _col.DataField = name;
                return this;
            }

            public ListDef.ColumnDef Build()
            {
                return _col;
            }
        }

        public ListDefBuilder<T> Column(string name, Action<ColumnDefBuilder<T>> act)
        {
            var cb = new ColumnDefBuilder<T>(name);
            act(cb);
            _curDef.Columns.Add(cb.Build());
            return this;
        }

        public ListDefBuilder<T> GetId(Func<T, object> getKey)
        {
            _curDef.GetId = obj => getKey((T) obj);
            return this;
        }

        public ListDefBuilder<T> GetDocRef(Func<T, string> getRef)
        {
            _curDef.GetDocRef = obj => getRef((T)obj);
            return this;
        }

        public ListDefBuilder<T> RowAttributes(Action<T, Dictionary<string, string>> act)
        {
            _curDef.GetRowAttributes = obj => {
                var attrs = new Dictionary<string, string>();
                act((T) obj, attrs);
                return attrs;
            };
            return this;
        }

        public ListDefBuilder<T> NHQuery(Func<IDictionary<string, object>, NHibernate.Criterion.ICriterion> builder)
        {
            _curDef.BuildSearchFilter = builder;
            return this;
        }

        public ListDef Build()
        {
            _curDef.DaoRecordType = typeof(T);
            if (_curDef.GetId == null)
                _curDef.GetId = x => SessionContext.CurrentSession.GetIdentifier(x);
            if (_curDef.BuildSearchFilter == null)
            {
                
            }
            return _curDef;
        }



        public static void Test()
        {
            var ld = new ListDefBuilder<CogDox.Core.BusinessObjects.BaseTask>()
                
                .GetId(x => x.Id)
                .GetDocRef(x => x.Id.ToString())
                .RowAttributes((tsk, attrs) => {
                    attrs["color"] = tsk.Status == BusinessObjects.TaskStatus.Completed ? "gray" : "active";
                })
                .Build();
        }
    }
}
