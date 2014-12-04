using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Security;
using Iris.DomainClasses.Entities;
using Newtonsoft.Json;

namespace Iris.Web.IrisMembership
{
    public class IrisIdentity : IIdentity
    {
        public IrisIdentity(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                Name = "Guest";
                Roles = new List<string> { "guest" };
                return;
            }

            var data = JsonConvert.DeserializeObject<IrisCookie>(ticket.UserData);

            if (data == null)
            {
                AsGuest();
                return;
            }

            UserName = data.UserName;
            //FirstName = data.FirstName;
            //LastName = data.LastName;
            Name = UserName;
            //Email = data.Email;
            Roles = data.Roles ?? new List<string> { "user" };
            RememberMe = data.RememberMe;

            //try
            //{
            //    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(data.TimeZone);
            //}
            //catch (Exception)
            //{
            //    TimeZone = TimeZoneInfo.Utc;
            //}
        }

        public IrisIdentity(User user)
        {
            if (user == null)
            {
                AsGuest();
                return;
            }

            UserName = user.UserName;
            //Email = user.Email;
            //FirstName = user.FirstName;
            //LastName = user.LastName;
            //Name = string.IsNullOrWhiteSpace(UserName) 
            //           ? user.Email
            //           : string.Format("{0} {1}", FirstName, LastName);

            Name = UserName;

            //try
            //{
            //    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
            //}
            //catch (Exception)
            //{
            //    TimeZone = TimeZoneInfo.Utc;
            //}

            Roles = new List<string> { user.Role.Name ?? "user" };
        }

        private void AsGuest()
        {
            Name = "Guest";
            Roles = new List<string> { "guest" };
        }

        public string UserName { get; set; }
        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public TimeZoneInfo TimeZone { get; set; }
        public bool RememberMe { get; set; }
        public IList<string> Roles { get; set; }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "IrisForms"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(UserName); }
        }

        public string Name { get; protected set; }

        #endregion
    }
}