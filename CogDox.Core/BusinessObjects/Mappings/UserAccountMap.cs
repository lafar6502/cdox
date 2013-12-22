using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class UserAccountMap : ClassMap<UserAccount>
    {
        public UserAccountMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='UserAccount'");
            DiscriminateSubClassesOnColumn("Subclass", 1);
            Map(x => x.Active).Not.Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
            Map(x => x.Email);
            Map(x => x.Login);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.PasswordHash);
            Map(x => x.PreferencesJson).CustomSqlType("ntext");
            Map(x => x.VoipPhone);
            Map(x => x.ExternalId);
            Map(x => x.LastModified).Not.Nullable();
            Map(x => x.LastSync).Nullable();
            Map(x => x.MobilePhone);
            HasManyToMany<GroupInfo>(x => x.MemberOf).Cascade.SaveUpdate().Table("Users2Groups");
        }
    }
}
