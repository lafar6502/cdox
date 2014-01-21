using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    public class ParameterModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type ParamType { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
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
        public bool ShowInMenu { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string ClickScript { get; set; }
        /// <summary>
        /// template used for rendering action's markup
        /// </summary>
        public string UITemplate { get; set; }
        public string ParentDocRef { get; set; }
        public List<ParameterModel> Parameters { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
