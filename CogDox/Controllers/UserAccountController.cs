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

namespace CogDox.Controllers
{
    public class UserAccountController : CogDoxControllerBase
    {
        [Authorize]
        public ActionResult UserList()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserList2()
        {
            return View();
        }

    }
}
