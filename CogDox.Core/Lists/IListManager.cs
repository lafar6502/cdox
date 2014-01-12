using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Core.Lists
{
    public class ListColumn
    {
        public string Id { get;set;}
        public string HeaderText { get;set;}
        public string DataField { get; set; }
        public bool Resizable { get; set; }
        public bool Sortable { get; set; }
        public int? Width { get; set; }
        public double? Flex { get; set; }
        /// <summary>
        /// never displayed
        /// </summary>
        public bool Invisible { get; set; }
        /// <summary>
        /// initially hidden, but can be shown
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// cell's css class
        /// </summary>
        public string Css { get; set; }
        /// <summary>
        /// grid-dependent data type
        /// </summary>
        public string DataType { get; set; }
    }

    public class ListModel
    {
        public string ListId { get;set;}
        public Type RecordType { get;set;}
        public List<ListColumn> Columns { get;set;}
    }

    public class ListRow
    {
        public List<object> Data { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class ListQueryResults
    {
        public IList<object> RawData { get; set; }
        public IList<ListRow> Rows { get; set; }
        public int Start { get; set; }
        public int Limit { get; set; }
        public bool HasMore { get; set; }
        public string Sort { get; set; }
        public bool SortAsc { get; set; }
        public ListModel List { get; set; }
    }

    public class ListQuery
    {
        public int Start { get;set;}
        public int Limit { get;set;}
        public string Sort { get;set;}
        public bool SortAsc { get;set;}
        public IDictionary<string, object> QueryParameters { get;set;}
    }

    public interface IListManager
    {
        IEnumerable<string> Lists { get; }
        ListModel GetModel(string listId);
        ListQueryResults Query(ListQuery lq, ListDef list);
        ListQueryResults Query(ListQuery lq, string listId);
    }
}
