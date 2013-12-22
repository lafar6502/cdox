using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CogDox.ITSM.BusinessObjects.Mappings
{
    public class IncidentMap : ClassMap<Incident>
    {
        public IncidentMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo");
            Map(x => x.Summary).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
            References(x => x.EndUser).Not.Nullable();
            References(x => x.Assignee).Not.Nullable();
        }
    }
}
