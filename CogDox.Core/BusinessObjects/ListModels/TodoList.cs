using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.Lists;
using NHibernate.Criterion;

namespace CogDox.Core.BusinessObjects.ListModels
{
    class TodoList
    {
        public void Test()
        {
            ListDef ld = new ListDef
            {
                ListId = "TODO"
            };
            ld.DaoRecordType = typeof(BaseTask);
            ld.BuildSearchFilter = (IDictionary<string, object> prm) => {
                var ses = SessionContext.CurrentSession;
                ses.CreateCriteria(ld.DaoRecordType);
                //var agl = 
                //Restrictions.Where<BaseTask>(x => x.AssigneeGroup.IsIn() && x.Status.IsIn(new object[] { TaskStatus.Assigned, TaskStatus.Executing, TaskStatus.InGroupQueue }));
                ld.Columns.Add(new ListDef.ColumnDef
                {
                    
                });
                
                return null;
            };
            
        }
    }
}
