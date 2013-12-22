using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    /// <summary>
    /// a model for GUI Action menu entry
    /// </summary>
    public class UIActionModel
    {
        public string Name { get; set; }
        public string ParentDocRef { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
