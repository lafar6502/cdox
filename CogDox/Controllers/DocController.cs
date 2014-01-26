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
using DM = CogDox.Core.DocManagement2;
using CogDox.Core.UI;

namespace CogDox.Controllers
{
    /// <summary>
    /// Controller for standard document details views
    /// </summary>
    public class DocController : CogDoxControllerBase
    {
        public DM.IDocumentRepository DocRepo { get; set; }

        public ActionResult Details(string id, string vname)
        {
            var vm = GetDocumentViewModel(id, vname);
            if (vm == null) return new HttpNotFoundResult();
            return View(vm.ViewTemplate, vm);
        }

        public ActionResult DetailsEmbed(string id, string vname)
        {
            var vm = GetDocumentViewModel(id, vname);
            if (vm == null) return new HttpNotFoundResult();
            if (ViewEngines.Engines.FindView(ControllerContext, vm.ViewTemplate + "_Embed", null) != null)
            {
                return View(vm.ViewTemplate + "_Embed", vm);
            }
            return View(vm.ViewTemplate, vm);
        }

        protected DocViewModelBase GetDocumentViewModel(string id, string vname)
        {
            var vm = DocRepo.GetDocumentViewModel(id, vname);
            foreach (var act in vm.Actions)
            {
                if (string.IsNullOrEmpty(act.UI_Id))
                {
                    act.UI_Id = MvcExtensions.GenID(this.HttpContext, "_docaction_");
                }
            }
            return vm;
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExecuteAction(string id, string actionName, string docVersion)
        {
            var prm = new Dictionary<string, object>();
            foreach (string p in Request.Form.Keys)
            {
                prm[p] = Request.Form[p];
            }
            var ret = DocRepo.ExecuteAction(id, actionName, prm, new DM.ActionOptions { DocVersion = docVersion });
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

    }
}
