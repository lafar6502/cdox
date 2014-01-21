using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.UI;

namespace CogDox.Core.DocManagement2
{
    public interface IDocumentRepository
    {
        
        DocViewModelBase GetDocumentViewModel(string docRef, string viewModelName = null);


    }
}
