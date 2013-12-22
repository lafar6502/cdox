using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CogDox.Models;
using CogDox.Core;
using CogDox.Core.Services;
using BL = Boo.Lang;
using BLI = Boo.Lang.Interpreter;
using NGinnBPM.MessageBus;

namespace CogDox.Controllers
{
    public class TestController : Controller
    {
        public IServiceResolver ServiceResolver { get; set; }

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

        

    }
}
