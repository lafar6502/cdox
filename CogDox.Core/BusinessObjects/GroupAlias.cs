using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    /// <summary>
    /// Group alias, for assigning alias names to groups
    /// Used for referring to groups from workflows
    /// </summary>
    public class GroupAlias
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual GroupInfo Group { get; set; }
    }
}
