#region

using System.Security.Principal;
using System.Web;
using System.Web.Security;

#endregion

namespace Iris.Web.IrisMembership
{
    public class IrisSupportPrincipalService : IPrincipalService
    {
        private readonly HttpContextBase _context;

        public IrisSupportPrincipalService(HttpContextBase context)
        {
            _context = context;
        }

        #region IPrincipalService Members

        public IPrincipal GetCurrent()
        {
            IPrincipal user = _context.User;
            // if they are already signed in, and conversion has happened
            if (user != null && user is IrisPrincipal)
                return user;

            // if they are signed in, but conversion has still not happened
            if (user == null || !user.Identity.IsAuthenticated || !(user.Identity is FormsIdentity))
                return new IrisPrincipal(new IrisIdentity((FormsAuthenticationTicket) null));
            var id = (FormsIdentity) _context.User.Identity;

            FormsAuthenticationTicket ticket = id.Ticket;
            if (FormsAuthentication.SlidingExpiration)
                ticket = FormsAuthentication.RenewTicketIfOld(ticket);

            var fid = new IrisIdentity(ticket);
            return new IrisPrincipal(fid);

            // not sure what's happening, let's just default here to a Guest
        }

        #endregion
    }
}