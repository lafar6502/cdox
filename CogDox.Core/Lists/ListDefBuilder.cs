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

        public ListDefBuilder()
        {
        }


        public class ColumnDefBuilder<T>
        {
            public ColumnDefBuilder(string name)
            {
            }

            public ColumnDefBuilder<T> Val(Func<T, object> getval)
            {
                return this;
            }

            public ColumnDefBuilder<T> Sortable(bool b)
            {
                return this;
            }

            public ColumnDefBuilder<T> Hidden(bool b = true)
            {
                return this;
            }

            public ListDef.ColumnDef Build()
            {
                ListDef.ColumnDef ld;
                
                throw new NotImplementedException();
            }
        }

        public ListDefBuilder<T> Column(Action<ColumnDefBuilder<T>> act)
        {
            return this;
        }

        public ListDefBuilder<T> GetId(Func<T, object> getKey)
        {
            _getId = getKey;
            return this;
        }

        public ListDefBuilder<T> GetDocRef(Func<T, string> getRef)
        {
            _getDocRef = getRef;
            return this;
        }

        public ListDefBuilder<T> RowAttributes(Action<T, Dictionary<string, string>> act)
        {
            _rowAttrs = act;
            return this;
        }

        public ListDef Build()
        {
            ListDef ld = new ListDef();
            ld.DaoRecordType = typeof(T);
            if (_getId == null)
                ld.GetId = x => SessionContext.CurrentSession.GetIdentifier(x);
            else 
                ld.GetId = x => _getId((T)x);
            if (_rowAttrs != null)
            {
                ld.GetRowAttributes = x => {
                    var rt = new Dictionary<string, string>();
                    _rowAttrs((T) x, rt);
                    return rt;
                };
            }
            return ld;
        }



        public static void Test()
        {
            var ld = new ListDefBuilder<CogDox.Core.BusinessObjects.BaseTask>()
                .Column(c =>
                {

                })
                .GetId(x => x.Id)
                .GetDocRef(x => x.Id.ToString())
                .RowAttributes((tsk, attrs) => {
                    attrs["color"] = tsk.Status == BusinessObjects.TaskStatus.Completed ? "gray" : "active";
                })
                .Build();
        }
    }
}
