using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace CogDox.Core.Services
{
    public interface IAuthenticateUsers
    {
        bool CheckPassword(string userName, string password);
        AppUser GetUserForPrincipal(IPrincipal appUser);
    }
}
