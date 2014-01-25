using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.BusinessObjects;

namespace CogDox.Core.DocManagement2
{
    [DocumentAction()]
    class AddComment : DocumentActionBase<BaseTask>
    {
        protected override object ExecuteAction(BaseTask doc, IDictionary<string, object> parameters)
        {
            ActionRecord ar = new ActionRecord(doc);
            ar.Action = ActionType.FindByCode("BaseTask.AddComment");
            ar.Summary = parameters["Comment"].ToString();
            SessionContext.CurrentSession.Save(ar);
            return null;
        }

        protected override bool CheckEnabled(BaseTask doc)
        {
            return true;
        }

        public override UI.UIActionModel GetModel(object doc)
        {
            var mdl = base.GetModel(doc);
            mdl.Parameters.Add(new UI.ParameterModel
            {
                Name = "Comment",
                ParamType = typeof(string),
                Value = ""
            });
            return mdl;
        }
    }
}
