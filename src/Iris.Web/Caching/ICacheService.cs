using Iris.Model;

namespace Iris.Web.Caching
{
    public interface ICacheService
    {
        SiteConfig GetSiteConfig();

        void RemoveSiteConfig();
    }
}
