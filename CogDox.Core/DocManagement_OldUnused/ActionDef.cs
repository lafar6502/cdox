using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement
{
    public class ActionDef
    {
        public virtual Type ApplicableToDocumentType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string Name
        {
            get { throw new NotImplementedException(); }
        }

        public virtual UIActionModel GetActionModel(object document)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsActionAllowed(object document)
        {
            throw new NotImplementedException();
        }

        public virtual object ExecuteAction(object document, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
            ActionBuilder<BusinessObjects.BaseTask>.New("TakeFromQueue")
                .DefineParameter<string>("comment", pd => 
                    pd.Required(true).DefaultValue(tsk => tsk.Summary))
                .When(x => x.Status == BusinessObjects.TaskStatus.InGroupQueue)
                .Execute(x =>
                {
                    x.Status = BusinessObjects.TaskStatus.Executing;
                    x.Assignee = BusinessObjects.UserAccount.CurrentUserAccount;
                    x.CurrentPersonAssignedDate = DateTime.Now;
                })
                .Build();
        }
    }

    public class ActionParamBuilder<T, TParam>
    {
        public ActionParamBuilder<T, TParam> DefaultValue(Func<T, TParam> fun)
        {
            return this;
        }

        public ActionParamBuilder<T, TParam> Required(bool required)
        {
            return this;
        }
    }

    public class ActionBuilder<T>
    {
        public static ActionBuilder<T> New(string actionName)
        {
            return new ActionBuilder<T>();
        }

        public ActionBuilder<T> When(Func<T, bool> f)
        {
            return this;
        }

        public ActionBuilder<T> Execute(Func<T, bool> f)
        {
            return this;
        }

        public ActionBuilder<T> Execute(Action<T> act)
        {
            return this;
        }

        public ActionBuilder<T> DefineParameter<TParam>(string name, Action<ActionParamBuilder<T, TParam>> act)
        {
            return this;
        }

        public ActionDef Build()
        {
            throw new NotImplementedException();
        }

    }
}
