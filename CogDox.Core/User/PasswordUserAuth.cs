using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CogDox.Core.BusinessObjects;
using CogDox.Core.Services;
using NHibernate;
using NHibernate.Linq;
using System.Security.Principal;
using NLog;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace CogDox.Core.User
{
    public class PasswordUserAuth : IAuthenticateUsers
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private ConcurrentDictionary<string, AppUser> _userCache = new ConcurrentDictionary<string, AppUser>();

        public bool CheckPassword(string userName, string password)
        {
            var usr = SessionContext.CurrentSession.Query<UserAccount>()
                .Where(x => x.Login == userName && x.Active == true).FirstOrDefault();
            if (usr == null) return false;
            var hash = ComputeHash(password);
            return usr.PasswordHash == hash || usr.PasswordHash == password;
        }


        public AppUser GetUserForPrincipal(System.Security.Principal.IPrincipal appUser)
        {
            string id = appUser.Identity.Name;
            var retUser = _userCache.GetOrAdd(id, x =>
            {
                var usr = SessionContext.CurrentSession.Query<UserAccount>()
                    .Where(u => u.Login == appUser.Identity.Name && u.Active).FirstOrDefault();
                if (usr == null)
                {
                    log.Info("User not found: {0}", appUser.Identity.Name);
                    return null;
                }
                var au = new AppUser { Id = usr.Id, Name = usr.Login };
                au.Permissions = new List<string>();
                au.MemberOf = new List<string>();
                List<int> mids = new List<int>();
                foreach (var gi in usr.MemberOf)
                {
                    au.MemberOf.Add(gi.Name);
                    mids.Add(gi.Id);
                    foreach (var p in gi.Permissions)
                    {
                        if (!au.Permissions.Contains(p.Name)) au.Permissions.Add(p.Name);
                    }
                }
                au.MyGroupIds = mids.ToArray();
                return au;
            });
            return retUser;
        }

        public static string ComputeHash(string password)
        {
            var md5 = MD5.Create();
            return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(password)));            
        }
    }
}
