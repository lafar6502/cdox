using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CogDox.Models
{
    public class DataTablesQueryResults
    {
        public int sEcho { get; set; }
        
        public int iTotalDisplayRecords { get; set; }
        public List<IEnumerable<object>> aaData { get; set; }
    }
}