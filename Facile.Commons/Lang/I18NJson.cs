using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading;
using NLog;
using System.IO;

namespace Facile.Commons.Lang
{
    class IJsonLangRC : ILangRC
    {
        public string BaseDir { get; set; }
        private Logger log = LogManager.GetCurrentClassLogger();
        private Dictionary<string, JObject> _dics = new Dictionary<string, JObject>();

        

        public string GetText(string id, string lang, string defaultText)
        {
            //log.Debug("GetText {0} {1}", id, lang);
            JObject j = null;
            if (_dics.TryGetValue(lang, out j))
            {
                string fn = Path.Combine(BaseDir, string.Format("Translation_{0}.json", lang));
                if (!File.Exists(fn))
                {
                    return defaultText == null ? id : defaultText;
                }
                DateTime dt = j.Value<DateTime>("_readDate");
                if (dt < File.GetLastWriteTime(fn)) j = null;
            }

            if (j == null)
            {
                string fn = Path.Combine(BaseDir, string.Format("Translation_{0}.json", lang));
                if (!File.Exists(fn))
                {
                    return defaultText == null ? id : defaultText;
                }
                else
                {
                    j = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(fn));
                    j["_readDate"] = DateTime.Now;
                }
                _dics[lang] = j;
            }
            string[] parts = id.Split('.');
            
            for(int i=0; i<parts.Length; i++)
            {
                string p = parts[i];
                var v = j[p];
                if (v == null)
                {
                    //log.Debug("Missing translation: {0}", id);
                    return defaultText == null ? id : defaultText;
                }
                if (i == parts.Length - 1)
                {
                    if (v is JObject)
                    {
                        string tv = v.Value<string>("_");
                        return tv;
                    }
                    return j.Value<string>(p);
                }
                else if (v is JObject)
                {
                    j = (JObject)v;
                }
                else throw new Exception("invalid lang file structure " + id);
            }
            return id;
        }

        public string GetText(string id)
        {
            return GetText(id, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, null);
        }
    }
}
