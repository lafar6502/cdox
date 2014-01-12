using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CogDox.Core.Lists;
using CogDox.Core.BusinessObjects;
using CogDox.Core;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Controllers
{
    public class DScriptController : CogDoxControllerBase
    {
        
        [ActionName("DynamicWithModel.js")]
        public ActionResult Dynamic(string message)
        {
            return this.JavaScriptFromView(model: message);
        }

        public ActionResult Index()
        {
            return Content("Scripts folder");
        }

        protected override void HandleUnknownAction(string actionName)
        {
            var mdl = new Models.CogDoxPageModel
            {
                ListManager = ListManager
            };
            var res = this.JavaScriptFromView(null, mdl);
            res.ExecuteResult(ControllerContext);
        }

    }
}
