using Iris.DomainClasses.Entities;

namespace Iris.Web.IrisMembership
{
    public interface IFormsAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
    }
}