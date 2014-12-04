using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Iris.DomainClasses.Entities;
using Newtonsoft.Json;

namespace Iris.Web.IrisMembership
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly HttpContextBase _httpContext;

        public FormsAuthenticationService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        #region IFormsAuthenticationService Members

        public void SignIn(User user, bool createPersistentCookie)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var cookie = new IrisCookie
            {
                UserName = user.UserName,
                RememberMe = createPersistentCookie,
                Roles = new List<string> { user.Role.Name ?? "user" }
            };

            string userData = JsonConvert.SerializeObject(cookie);
            var ticket = new FormsAuthenticationTicket(1, cookie.UserName, DateTime.Now,
                                                       DateTime.Now.Add(FormsAuthentication.Timeout),
                                                       createPersistentCookie, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { Expires = DateTime.Now.Add(FormsAuthentication.Timeout) };

            _httpContext.Response.Cookies.Add(httpCookie);
        }

        public void SignOut()
        {
            // Not worth covering, has been tested by Microsoft
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}