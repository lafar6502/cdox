﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.IO;

namespace System.Web.Mvc
{
    /// 






    /// Mvc extensions for dynamic CSS and JS    /// 


    public static class MvcExtensions
    {
        /// 

        public static string GenID(this HtmlHelper helper, string prefix= null)
        {
            return GenID(helper.ViewContext.HttpContext);
        }


        public static string GenID(HttpContextBase ctx, string prefix = null)
        {
            int cid = ctx.Items.Contains("_cd_uid") ? (int)ctx.Items["_cd_uid"] : 1;
            ctx.Items["_cd_uid"] = ++cid;
            return prefix == null ? "_u_" + cid : prefix + cid;
        }




        /// CSS content result rendered by partial view specified        /// 
        /// "controller">current controller
        /// "cssViewName">view name, which contains partial view with one STYLE block only
        /// "model">optional model to pass to partial view for rendering
        /// 
        public static ActionResult CssFromView(this Controller controller, string cssViewName = null, object model = null)
        {
            var cssContent = ParseViewToContent(controller, cssViewName, "style", model);
            if (cssContent == null) throw new HttpException(404, "CSS not found");
            return new ContentResult() { Content = cssContent, ContentType = "text/css" };
        }

        /// 









        /// Javascript content result rendered by partial view specified        /// 
        /// "controller">current controller
        /// "javascriptViewName">view name, which contains partial view with one SCRIPT block only
        /// "model">optional model to pass to partial view for rendering
        /// 
        public static ActionResult JavaScriptFromView(this Controller controller, string javascriptViewName = null, object model = null)
        {
            var jsContent = ParseViewToContent(controller, javascriptViewName, "script", model);
            if (jsContent == null) throw new HttpException(404, "JS not found");
            return new JavaScriptResult() { Script = jsContent };
        }

        /// 

        private static Regex _stripScriptRe = new Regex(string.Format("<{0}[^>]*>(.*?)</{0}>", "script"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    
        public static string StripScriptTags(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var res = _stripScriptRe.Match(s);
            if (res.Success && res.Groups.Count > 1)
                return res.Groups[1].Value;
            return s;
        }

        public static string StripScriptTags(HtmlString s)
        {
            return StripScriptTags(s.ToHtmlString());
        }






        /// Parse view and render it to a string, trimming specified HTML tag        /// 
        /// "controller">controller which renders the view
        /// "viewName">name of cshtml file with content. If null, then actionName used
        /// "tagName">Content rendered expected to be wrapped with this html tag, and it will be trimmed from result
        /// "model">model to pass for view to render
        /// 
        static string ParseViewToContent(Controller controller, string viewName, string tagName, object model = null)
        {
            using (var viewContentWriter = new StringWriter())
            {
                if (model != null)
                    controller.ViewData.Model = model;

                if (string.IsNullOrEmpty(viewName))
                    viewName = controller.RouteData.GetRequiredString("action");

                
                var viewResult = new ViewResult()
                {
                    ViewName = viewName,
                    ViewData = controller.ViewData,
                    TempData = controller.TempData,
                    ViewEngineCollection = ViewEngines.Engines
                };

                var viewEngineResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                if (viewEngineResult.View == null)
                    return null;

                try
                {
                    var viewContext = new ViewContext(controller.ControllerContext, viewEngineResult.View, controller.ViewData, controller.TempData, viewContentWriter);
                    viewEngineResult.View.Render(viewContext, viewContentWriter);
                    var viewString = viewContentWriter.ToString().Trim('\r', '\n', ' ');
                    var regex = string.Format("<{0}[^>]*>(.*?)</{0}>", tagName);
                    var res = Regex.Match(viewString, regex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline);
                    if (res.Success && res.Groups.Count > 1)
                        return res.Groups[1].Value;
                    else throw new InvalidProgramException(string.Format("Dynamic content produced by viewResult '{0}' expected to be wrapped in '{1}' tag", viewName, tagName));
                }
                finally
                {
                    if (viewEngineResult.View != null)
                        viewEngineResult.ViewEngine.ReleaseView(controller.ControllerContext, viewEngineResult.View);
                }
            }

        }

        /// <summary>
        /// Zwraca vname jesli view istnieje, w przeciwnym razie defaultView
        /// </summary>
        /// <param name="hh"></param>
        /// <param name="vname"></param>
        /// <param name="defaultView"></param>
        /// <returns></returns>
        public static string DefaultPartial(this HtmlHelper hh, string vname, string defaultView)
        {
            if (string.IsNullOrEmpty(vname)) return defaultView;
            var vv = ViewEngines.Engines.FindPartialView(hh.ViewContext.Controller.ControllerContext, vname);
            if (vv == null || vv.View == null) return defaultView;
            return vname;
        }

        public static MvcHtmlString RenderAllPartialsMatching(this HtmlHelper hh, string pattern, string inDirectory)
        {
            StringWriter sw = new StringWriter();

            string pth = hh.ViewContext.HttpContext.Server.MapPath(inDirectory);
            foreach (string file in Directory.GetFiles(pth, pattern))
            {
                string fn = Path.GetFileName(file);
                string tname = inDirectory + "/" + fn;
                var s = hh.Partial(tname);
                sw.WriteLine(s.ToString());
            }
            return new MvcHtmlString(sw.ToString());
        }

        public static string DefaultViewName(this HtmlHelper hh, string vname, string defaultView)
        {
            if (string.IsNullOrEmpty(vname)) return defaultView;
            var vv = ViewEngines.Engines.FindView(hh.ViewContext.Controller.ControllerContext, vname, null);
            if (vv == null || vv.View == null) return defaultView;
            return vname;
        }

        public static string DefaultViewName(this Controller c, string vname, string defaultView)
        {
            var vv = ViewEngines.Engines.FindView(c.ControllerContext, vname, null);
            if (vv == null || vv.View == null) return defaultView;
            return vname;
        }


    }
}
