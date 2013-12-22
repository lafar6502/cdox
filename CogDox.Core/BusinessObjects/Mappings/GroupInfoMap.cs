using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class GroupInfoMap : ClassMap<GroupInfo>
    {
        public GroupInfoMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='GroupInfo'");
            Map(x => x.Name).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.IsProcessGroup).Not.Nullable();
            Map(x => x.DefaultDeadlineMinutes).Nullable();
            Map(x => x.ExtId).Nullable();
            References<UserAccount>(x => x.Supervisor).Nullable();
            HasManyToMany<UserAccount>(x => x.Members).Cascade.SaveUpdate().Table("Users2Groups");
            HasManyToMany<UserAccount>(x => x.Leaders).Cascade.SaveUpdate().Table("Leaders2Groups");
            HasManyToMany<Permission>(x => x.Permissions).Cascade.SaveUpdate().Table("Groups2Permissions");
        }
    }
}
