using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.UI
{
    public enum FieldAccess
    {
        ReadOnly = 1,
        ReadWrite = 2,
        Required = 3
    }

    public class FieldModel
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
        public FieldAccess Access { get; set; }

        /// <summary>
        /// List of possible options (optional)
        /// </summary>
        public List<IdLabel> Options { get; set; }
        /// <summary>
        /// Data source name for auto-complete fields
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// UI ID
        /// </summary>
        public string Id { get; set; }
    }

    public class IdLabel
    {
        public string Id { get; set; }
        public string Label { get; set; }
    }

    


}
