﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.BusinessObjects;

namespace CogDox.Core.DocManagement2
{
    class AddComment : DocumentActionBase<BaseTask>
    {
        protected override object ExecuteAction(BaseTask doc, IDictionary<string, object> parameters)
        {
            ActionRecord ar = new ActionRecord(doc);
            ar.Action = ActionType.FindByCode("AddComment");
            ar.Summary = parameters["Comment"].ToString();
            SessionContext.CurrentSession.Save(ar);
            return null;
        }

        protected override bool CheckEnabled(BaseTask doc)
        {
            return true;
        }
    }
}