using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace CogDox.Core.BusinessObjects.Mappings
{
    class BaseTaskMap : ClassMap<BaseTask>
    {
        public BaseTaskMap()
        {
            Table("Task");
            Id(x => x.Id).GeneratedBy.HiLo("HiLo", "NextVal", "10", "HKey='Task'");
            DiscriminateSubClassesOnColumn("Subclass", 1);
            Map(x => x.CreatedDate).Not.Nullable();
            Map(x => x.CompletedDate).Nullable();
            Map(x => x.CompletionCode).Nullable();
            Map(x => x.Status).CustomType<TaskStatus>().Not.Nullable();
            Map(x => x.CurrentGroupAssignedDate).Not.Nullable();
            Map(x => x.CurrentPersonAssignedDate).Nullable();
            Map(x => x.Description).Nullable().CustomSqlType("ntext");
            Map(x => x.Summary).Nullable();
            Map(x => x.ExternalId).Nullable();
            Map(x => x.ExternalProcessId).Nullable();
            Map(x => x.ExternalTaskCategoryId).Nullable();
            Map(x => x.ResumeDate).Nullable();
            Map(x => x.SolutionText).Nullable().CustomSqlType("ntext");
            Map(x => x.TODOList).Not.Nullable();
            Map(x => x.TaskDataJson).Nullable().CustomSqlType("ntext");
            Version(x => x.Version).Generated.Always();
            References<UserAccount>(x => x.Assignee).Nullable();
            References<GroupInfo>(x => x.AssigneeGroup).Not.Nullable();
            References<TaskProfile>(x => x.Profile).Not.Nullable();
        }
    }
}
