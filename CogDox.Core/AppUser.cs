using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace CogDox.Core
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> MemberOf { get; set; }
        public List<string> Permissions { get; set; }
        public int[] MyGroupIds { get; set; }

        public bool HasPermission(string name)
        {
            return Permissions != null && (Permissions.Contains(name) || Permissions.Contains("*"));
        }


    }

    public class UserContext
    {
        [ThreadStatic]
        private static AppUser _curUser;

        public static AppUser CurrentUser
        {
            get { return _curUser; }
            set { _curUser = value; }
        }

        public static AppUser SystemUser
        {
            get { return new AppUser { Id = 1, Name = "System" }; }
        }

        public static bool UserHasPermission(string name) 
        {
            return CurrentUser != null && CurrentUser.HasPermission(name);
        }


    }
}
