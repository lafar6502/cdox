using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facile.Commons.Lang;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Facile.Commons
{
    public class I18N
    {
        protected static ILangRC _langRc;

        private static void InitIfNecessary()
        {
            if (_langRc == null)
            {
                _langRc = new Lang.IJsonLangRC
                {
                    BaseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data")
                };
            }
        }

        public static string GetText(object v)
        {
            if (v is string) return GetText((string)v);
            if (v.GetType().IsEnum)
            {
                return GetText(v.GetType().Name + "." + v.ToString());
            }
            throw new NotImplementedException();
        }

        public static string GetText(string id, string defaultText = null)
        {
            InitIfNecessary();
            string lang = "PL";// Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return _langRc.GetText(id, lang);
        }
    }

}
