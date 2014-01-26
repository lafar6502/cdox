using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.DocManagement2;
using CogDox.Core.BusinessObjects;
using NHibernate;
using NGinnBPM.MessageBus;

namespace CogDox.Core.AppLogic.Task
{
    [DocumentAction]
    public class ResetPassword : DocumentActionBase<UserAccount>
    {
        public IMessageBus MessageBus { get; set; }
        public void ResetUserPassword(UserAccount ua)
        {
            ua.PasswordHash = "afdkau90as090sa";
            ActionRecord ar = new ActionRecord(ua);
            ar.Action = ActionType.FindByCode("UserAccount.AddComment");
            ar.Summary = "Reset password";
            SessionContext.CurrentSession.Save(ar);
        }

        protected override object ExecuteAction(UserAccount doc, IDictionary<string, object> parameters)
        {
            ResetUserPassword(doc);
            return null;
        }

        protected override bool CheckEnabled(UserAccount doc)
        {
            return true;
        }

        public override UI.UIActionModel GetModel(object doc)
        {
            var vm = base.GetModel(doc);
            vm.ShowInMenu = true;
            vm.UITemplate = "Actions/UserAccount.ResetPassword";
            vm.MenuActionType = "showmodal";
            vm.Parameters.Add(new UI.FieldModel
            {
                Name = "comment",
                ParamType = typeof(string),
                FieldType = "textarea"
            });
            return vm;
        }
    }
}
