using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core.BusinessObjects;

namespace CogDox.Models
{
    public class TaskDetailsModel
    {
        public BaseTask Task { get; set; }
        /// <summary>
        /// javascript function name or expression that
        /// will return details view hosting API.
        /// This api currently supports the following functions:
        /// -- reload - will refresh current view
        /// </summary>
        public string ViewHostCallbackJS { get; set; }
    }
}