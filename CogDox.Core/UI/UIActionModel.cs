using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    /// <summary>
    /// a model for GUI Action menu entry
    /// 
    /// what kinds of actions do we have?
    /// - links (normal links)
    /// - modal windows (link + toggle-modal)
    /// - javascript function
    /// </summary>
    public class UIActionModel
    {
        public string Action { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string ClickScript { get; set; }
        public string UITemplate { get; set; }
        public string ParentDocRef { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
