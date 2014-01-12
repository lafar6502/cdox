using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CogDox.Models;
using CogDox.Core;
using CogDox.Core.Lists;
using CogDox.Core.Services;
using BL = Boo.Lang;
using BLI = Boo.Lang.Interpreter;
using NGinnBPM.MessageBus;
using NHibernate;
using NHibernate.Criterion;
using CogDox.Core.BusinessObjects;

namespace CogDox.Controllers
{
    public class TestController : CogDoxControllerBase
    {
        
        [Authorize]
        public ActionResult Console(string eval)
        {
            if (!string.IsNullOrEmpty(eval))
            {
                BLI.InteractiveInterpreter ii = new BLI.InteractiveInterpreter();
                ii.Ducky = true;
                var cc = ii.Eval(@"
import CogDox.Core
import CogDox.Core.BusinessObjects
import NHibernate
import NHibernate.Criterion
import NLog");
                if (cc.Errors.Count > 0) return View(cc);
                ii.SetValue("HTRequest", Request);
                ii.SetValue("User", UserContext.CurrentUser);
                ii.SetValue("Session", SessionContext.CurrentSession);
                ii.SetValue("Container", ServiceResolver);
                cc = ii.Eval(eval);
                return View(cc);
            }
            return View();
        }

        public ActionResult FormBuilder()
        {
            return View();
        }

        public ActionResult DiagramEditor()
        {
            return View();
        }

        [Authorize]
        public ActionResult TodoList()
        {
            
            ListQuery lq = new ListQuery
            {
                Start = 0,
                Limit = 20,
                QueryParameters = new Dictionary<string, object>(),
                Sort = null
            };
            var result = ListManager.Query(lq, "TODO");


            return View(new Models.ListSearchModel
            {
                List = result.List,
                Query = lq,
                Results = result
            });
        }

    }
}
