using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.Lists;
using CogDox.Core;
using CogDox.Core.BusinessObjects;
using NHibernate.Criterion;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //var qn = NHDynQuery.BuildQuery(x => x.Login.Like("jozek") && x.Active == true);

            var ses = SessionContext.CurrentSession;
            //Console.WriteLine(qn.ToString());
            var crit = Restrictions.Where<BaseTask>(x => x.AssigneeGroup.Id.IsIn(new object[] {1, 2, 3, 4}) && x.Status.IsIn(new object[] { TaskStatus.Assigned, TaskStatus.Executing, TaskStatus.InGroupQueue }));
            

                //Restrictions.Where<BaseTask>(x => x.Assignee.Id == 15 && x.AssigneeGroup.Id == 10 && x.Status == TaskStatus.Assigned);
            Console.WriteLine(crit.ToString());
        }
    }
}
