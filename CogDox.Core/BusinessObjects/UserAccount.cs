using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogDox.Core.BusinessObjects
{
    public class UserAccount
    {
        public virtual int Id { get; set; }
        public virtual int Subclass { get; set; }
        public virtual string Name { get; set; }
        public virtual string Login { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string Email { get; set; }
        public virtual string VoipPhone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string ExternalId { get; set; }
        public virtual bool Active { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? LastSync { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual Dictionary<string, object> Preferences { get; set; }
        public virtual string PreferencesJson { get; set; }

        public virtual IList<GroupInfo> MemberOf { get; set; }

        /// <summary>
        /// Current user's account
        /// </summary>
        public static UserAccount CurrentUserAccount
        {
            get
            {
                var ses = SessionContext.CurrentSessionRequired;
                if (UserContext.CurrentUser == null) return null;
                return ses.Get<UserAccount>(UserContext.CurrentUser.Id);
            }
        }

        public static UserAccount FindUserByQuery(string qry, GroupInfo group)
        {
            if (qry.StartsWith(":"))
            {
                switch (qry.ToLower())
                {
                    case ":group_supervisor": return group.Supervisor;
                    default: throw new Exception("unknown: " + qry);
                }
            }
            var qr = SessionContext.CurrentSession.QueryOver<UserAccount>()
                    .Where(x => x.Active && (x.Login == qry || x.Email == qry || x.ExternalId == qry));
            if (group != null) qr = qr.Where(x => x.MemberOf.Contains(group));
            var pl = qr.OrderBy(x => x.Id).Asc.Take(1).List();
            return pl.Count > 0 ? pl[0] : null;
        }
    }
}
