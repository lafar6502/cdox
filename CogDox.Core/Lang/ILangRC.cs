using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Lang
{
    public interface ILangRC
    {
        string GetText(string id, string lang, string defaultText = null);
    }
}
