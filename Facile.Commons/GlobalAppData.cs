using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGinnBPM.MessageBus;

namespace Facile.Commons
{
    public class GlobalAppData
    {
        public static IServiceResolver Container { get; set; }
    }
}
