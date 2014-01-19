﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class ActionType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }


        public virtual ActionType CreateObject
        {
            get { return SessionContext.CurrentSession.Get<ActionType>(1); }
        }


    }
}
