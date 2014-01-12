using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core;
using CogDox.Core.Lists;
using CogDox.Core.Services;

namespace CogDox.Models
{
    public class CogDoxPageModel
    {
        public IListManager ListManager { get; set; }
        
    }
}