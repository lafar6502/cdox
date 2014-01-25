using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CogDox.Core.UI;

namespace CogDox.Models
{
    public class FieldModel
    {
        public string Id { get; set; }
        ParameterModel Parameter { get; set; }
    }
}