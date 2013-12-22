using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class ObjectClassMap : ClassMap<ObjectClass>
    {
        public ObjectClassMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable();
        }
    }
}
