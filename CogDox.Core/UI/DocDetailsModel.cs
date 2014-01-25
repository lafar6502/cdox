using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    /// <summary>
    /// Single document view model (base class)
    /// View model providers can extend this class to provide additional 
    /// data to the document view.
    /// </summary>
    public class DocViewModelBase
    {
        public List<UIActionModel> Actions { get; set; }
        /// <summary>
        /// Raw document
        /// </summary>
        public object Document { get; set; }
        /// <summary>
        /// Document reference
        /// </summary>
        public string DocRef { get; set; }
        /// <summary>
        /// View template name (form name etc)
        /// </summary>
        public string ViewTemplate { get; set; }
        /// <summary>
        /// Document version, if versioning is supported
        /// </summary>
        public string DocVersion { get; set; }
    }
}
