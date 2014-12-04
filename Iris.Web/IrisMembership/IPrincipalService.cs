using System.Security.Principal;

namespace Iris.Web.IrisMembership
{
    public interface IPrincipalService
    {
        IPrincipal GetCurrent();
    }
}