using System;
using System.Linq;
using System.Security.Principal;

namespace Iris.Web.IrisMembership
{
    public class IrisPrincipal : IPrincipal
    {
        private readonly IrisIdentity _identity;

        public IrisPrincipal(IrisIdentity identity)
        {
            _identity = identity;
        }

        #region IPrincipal Members

        public bool IsInRole(string role)
        {
            return
                _identity.Roles.Any(
                    current => string.Compare(current, role, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public IrisIdentity Information
        {
            get { return _identity; }
        }

        public bool IsUser
        {
            get { return !IsGuest; }
        }

        public bool IsGuest
        {
            get { return IsInRole("guest"); }
        }

        #endregion
    }
}