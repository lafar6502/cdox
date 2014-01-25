using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core;
using CogDox.Core.DocManagement2;
using CogDox.Core.BusinessObjects;

namespace CogDox.Core.AppLogic.Task
{
    public class ResumeTask : DocumentActionBase<BaseTask>
    {
        public static void Resume(BaseTask bt, bool returnToGroup, string comment)
        {
            throw new NotImplementedException();
        }

        protected override object ExecuteAction(BaseTask doc, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        protected override bool CheckEnabled(BaseTask doc)
        {
            throw new NotImplementedException();
        }
    }
}
