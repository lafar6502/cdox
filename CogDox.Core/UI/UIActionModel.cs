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
        /// <summary>
        /// UI field type. This field selects the UI template to be used.
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// Field required
        /// </summary>
        public bool Required { get; set; }
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
        /// <summary>
        /// show action in menu
        /// </summary>
        public bool ShowInMenu { get; set; }
        /// <summary>
        /// menu label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// menu entry tooltip
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// Menu action type. 
        /// </summary>
        public string MenuActionType { get; set; }
        /// <summary>
        /// Menu click script. Depends on action.
        /// </summary>
        public string ClickScript { get; set; }
        /// <summary>
        /// template used for rendering action's markup
        /// </summary>
        public string UITemplate { get; set; }
        public string ParentDocRef { get; set; }
        /// <summary>
        /// action parameters
        /// </summary>
        public List<ParameterModel> Parameters { get; set; }
        /// <summary>
        /// Some additional data
        /// </summary>
        public Dictionary<string, object> Data { get; set; }
    }
}
