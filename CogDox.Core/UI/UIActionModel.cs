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
        public List<FieldModel> Parameters { get; set; }
        /// <summary>
        /// Some additional data
        /// </summary>
        public Dictionary<string, object> Data { get; set; }
        /// <summary>
        /// temporary UI identifier
        /// </summary>
        public string UI_Id { get; set; }
    }
}
