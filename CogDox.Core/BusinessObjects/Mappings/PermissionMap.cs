using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class PermissionMap : ClassMap<Permission>
    {
        public PermissionMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
        }
    }
}
