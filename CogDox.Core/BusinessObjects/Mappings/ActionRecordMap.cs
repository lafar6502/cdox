using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class ActionRecordMap : ClassMap<ActionRecord>
    {
        public ActionRecordMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='ActionRecord'");
            Map(x => x.ParentId).Not.Nullable();
            Map(x => x.Summary);
            Map(x => x.TimeStamp).Not.Nullable();
            Map(x => x.NewId);
            Map(x => x.PrevId);
            Map(x => x.RefId);
            Map(x => x.RefStr);
            References<UserAccount>(x => x.User).Not.Nullable();
            References<ObjectClass>(x => x.ParentClass).Not.Nullable();
            References<ActionType>(x => x.Action).Not.Nullable();
        }
    }
}
