using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core.Lists;

namespace CogDox.Models
{
    public class ListSearchModel
    {
        public ListQueryResults Results { get; set; }
        public ListModel List { get; set; }
        public ListQuery Query { get; set; }

        public string ResultRowTemplate { get; set; }
        public string HeaderTemplate { get; set; }
        public string SearchFormTemplate { get; set; }
    }
}