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
    public class AssignToGroups : DocumentActionBase<UserAccount>
    {
        public IMessageBus MessageBus { get; set; }

        public void AssignUserToGroups(UserAccount ua, int[] newMembership, string comment)
        {
            var ses = SessionContext.CurrentSession;
            List<GroupInfo> lg = new List<GroupInfo>();
            foreach (int gid in newMembership)
            {
                lg.Add(ses.Get<GroupInfo>(gid));
            }
            ua.MemberOf = lg;
            ses.Update(ua);
        }

        protected override bool CheckEnabled(UserAccount doc)
        {
            return true;
        }

        protected override object ExecuteAction(UserAccount doc, IDictionary<string, object> parameters)
        {
            AssignUserToGroups(doc, (int[])parameters["newMembership"], (string)parameters["comment"]);
            return null;
        }


        public override UI.UIActionModel GetModel(object doc)
        {
            var md = base.GetModel(doc);
            md.UITemplate = "Actions/UserAccount.AssignToGroups";
            return md;

        }

    }
}
