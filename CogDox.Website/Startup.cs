using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(CogDox.Website.CogdoxStartup))]
namespace CogDox.Website
{
    public class CogdoxStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            
        }
    }
}