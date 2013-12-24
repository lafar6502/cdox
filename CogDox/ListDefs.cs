using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core;
using CogDox.Core.Lists;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using CogDox.Core.Services;
using CogDox.Core.BusinessObjects;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CogDox
{
    public class ListDefs
    {
        public static IEnumerable<ListDef> GetListDefinitions()
        {
            List<ListDef> defs = new List<ListDef>();
            defs.Add(TodoList());

            return defs;
        }

        private static ListDef TodoList()
        {
            ListDef ld = new ListDef
            {
                ListId = "TODO",
                DaoRecordType = typeof(BaseTask),
                GetId = x => ((BaseTask)x).Id,
                BuildSearchFilter = prm =>
                {
                    var r = Restrictions.Where<BaseTask>(x => x.TODOList && x.AssigneeGroup.IsIn(UserContext.CurrentUser.MyGroupIds) && x.Status.IsIn(new object[] { TaskStatus.Assigned, TaskStatus.Executing, TaskStatus.InGroupQueue }));
                    return r;
                }
            };
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef {
                DataField = "Id",
                HeaderText = "Id"
            }, x => x.Id);
            ld.Columns.Add(new ListDef.ColumnDef
            {
                DataField = "CreatedDate",
                HeaderText = "CreatedDate",
                Sortable = true,
                GetVal = x => ((BaseTask)x).CreatedDate
            });
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Status",
                HeaderText = I18N.GetText("BaseTask.Status")
            }, x => I18N.GetText(x.Status));
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Summary",
                HeaderText = I18N.GetText("BaseTask.Summary"),
                Sortable = true
            }, x => x.Summary);

            return ld;
        }
    }
}