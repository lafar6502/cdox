using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class GroupAliasMap : ClassMap<GroupAlias>
    {
        public GroupAliasMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='GroupAlias'");
            Map(x => x.Name).Unique().Not.Nullable();
            References(x => x.Group).Not.Nullable();
        }
    }
}
