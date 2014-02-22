using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Facile.Commons;

namespace Facile.MvcServices
{
    public class MvcContextRequestContextResolver : Facile.Commons.RequestContext.IContextResolver
    {
        public Commons.RequestContext GetCurrentContext()
        {
            return HttpContext.Current.Items["_fac_requestContext"] as RequestContext;
        }

        public void SetCurrentContext(Commons.RequestContext rc)
        {
            if (rc == null)
            {
                HttpContext.Current.Items.Remove("_fac_requestContext");
                return;
            }
            HttpContext.Current.Items["_fac_requestContext"] = rc;
        }
    }
}
