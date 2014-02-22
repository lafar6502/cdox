using System.Web;
using System.Web.Mvc;
using Facile.Commons;

namespace Facile.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }
    }
}