using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;
using CogDox.Core.Lists;

namespace CogDox.Core.DocManagement
{
    public interface IDocViewProvider
    {
        DocViewModelBase GetDetails(string docRef);

    }
}
