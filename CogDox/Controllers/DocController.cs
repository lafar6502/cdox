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
            return View(vm.ViewTemplate, vm.Document);
        }

        public ActionResult DetailsEmbed(string id, string vname)
        {
            var vm = GetDocumentViewModel(id, vname);
            if (vm == null) return new HttpNotFoundResult();
            if (ViewEngines.Engines.FindView(ControllerContext, vm.ViewTemplate + "_Embed", null) != null)
            {
                return View(vm.ViewTemplate + "_Embed", vm.Document);
            }
            return View(vm.ViewTemplate, vm.Document);
        }

        protected DocViewModelBase GetDocumentViewModel(string id, string vname)
        {
            return DocRepo.GetDocumentViewModel(id, vname);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExecuteAction(string docRef, string action)
        {
            
            throw new NotImplementedException();
        }

    }
}
