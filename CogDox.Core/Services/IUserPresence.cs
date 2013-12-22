using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.Services
{
    /// <summary>
    /// User presence registry
    /// </summary>
    public interface IUserPresence
    {
        IEnumerable<string> SesssionIds { get; }
        bool IsUserLoggedOn(int userId);
        IEnumerable<string> GetUserSessions(int userId);
    }
}
