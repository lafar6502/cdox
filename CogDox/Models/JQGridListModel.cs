using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core.Lists;

namespace CogDox.Models
{
    public class JQGridListModel
    {
        public ListModel List { get; set; }
        public string ListDivId { get; set; }
        public string FilterTemplate { get; set; }
        public string ResultTemplate { get; set; }
        public object Options { get; set; }
    }
}