using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class TaskProfileMap : ClassMap<TaskProfile>
    {
        public TaskProfileMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='TaskProfile'");
            Map(x => x.Name).Not.Nullable().Unique();
            Map(x => x.CanUsersAssignTask).Not.Nullable();
            Map(x => x.CanUsersReassignToAnotherGroup).Not.Nullable();
            Map(x => x.CanUsersRejectTask).Not.Nullable();
            Map(x => x.CanUsersSuspendTask).Not.Nullable();
            Map(x => x.IsDocumentAction).Not.Nullable();
            Map(x => x.ShowOnTODOList).Not.Nullable();
        }
    }
}
