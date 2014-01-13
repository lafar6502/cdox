using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    public class DocDetailsModel
    {
        public List<UIActionModel> Actions { get; set; }
        public object Document { get; set; }
        public string DocRef { get; set; }
        public string ViewName { get; set; }
    }
}
