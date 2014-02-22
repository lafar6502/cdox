using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIO = System.IO;
using System.Text;
using Facile.Commons;
using NLog;

namespace Facile.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static Logger log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            log.Info("rc: {0}", RequestContext.Current);
            var ses = RequestContext.Current.Session;
            log.Info("ses: {0}", RequestContext.Current.Session);
            
            return View();
        }

        public ActionResult Funkcje()
        {
            return View();
        }

        public ActionResult VDoc(string id)
        {
            string fl = Server.MapPath(Url.Content("~/Views/Home/" + id));
            return View((object) System.IO.File.ReadAllText(fl, Encoding.UTF8));
        }
    }
}
