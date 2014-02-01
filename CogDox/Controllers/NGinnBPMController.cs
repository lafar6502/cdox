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
using NGinnBPM.Runtime;

namespace CogDox.Controllers
{
    /// <summary>
    /// Controller for standard document details views
    /// </summary>
    public class NGinnBPMController : CogDoxControllerBase
    {
        public ProcessRunner Runner { get; set; }
        public IProcessPackageRepo PackageRepo { get; set; }
        
        [Authorize]
        public ActionResult Packages()
        {
            return View(PackageRepo.PackageNames);
        }

        [Authorize]
        public ActionResult Package(string id)
        {
            IProcessPackage pp = PackageRepo.GetProcessPackage(id);
            return View(pp);
        }

        [Authorize]
        public ActionResult ProcessDef(string id)
        {
            var pd = PackageRepo.GetProcessDef(id);
            return View(pd);
        }

        [HttpGet]
        [Authorize]
        public ActionResult StartProcess(string id)
        {
            var pd = PackageRepo.GetProcessDef(id);
            return View(pd);
        }

        [HttpPost]
        [Authorize]
        public ActionResult StartProcessPost()
        {
            throw new NotImplementedException();
        }

    }
}
