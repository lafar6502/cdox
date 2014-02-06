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
            defs.Add(UserList());
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
                DataType = "numeric",
                HeaderText = "Id",
                Sortable = true
            }, x => x.Id);
            ld.Columns.Add(new ListDef.ColumnDef
            {
                DataField = "CreatedDate",
                DataType = "date",
                HeaderText = "CreatedDate",
                Sortable = true,
                GetVal = x => ((BaseTask)x).CreatedDate
            });
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Status",
                DataType = "text",
                HeaderText = I18N.GetText("BaseTask.Status"),
                Sortable = true
            }, x => I18N.GetText(x.Status));
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "Summary",
                DataType = "text",
                HeaderText = I18N.GetText("BaseTask.Summary"),
                Sortable = false, Flex = 1.0
            }, x => x.Summary);
            ld.AddColumn<BaseTask>(new ListDef.ColumnDef
            {
                DataField = "AssigneeGroup",
                HeaderText = I18N.GetText("BaseTask.AssigneeGroup"),
                Sortable = true
            }, x => x.AssigneeGroup.Name);

            return ld;
        }

        private static ListDef UserList()
        {
            return new ListDefBuilder<UserAccount>()
                .Column("Id", cb => {
                    cb.Val(x => x.Id).Sortable(true);
                })
                .Column("Name", cb => {
                    cb.Val(x => x.Name).Sortable(true);
                })
                .Column("Login", cb => {
                    cb.Val(x => x.Login).Sortable(true);
                })
                .Column("Email", cb => {
                    cb.Val(x => x.Email).Sortable(true);
                })
                .Column("MobilePhone", cb => {
                    cb.Val(x => x.MobilePhone).Sortable(true);
                })
                .NHQuery(param => {
                    return null;
                })
                .GetId(x => x.Id)
                .Build();
        }
    }
}