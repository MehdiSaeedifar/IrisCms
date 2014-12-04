using System;
using System.Configuration;
using System.Web;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Infrastructure;

namespace Iris.Web.Caching
{
    public class IrisCache
    {
        public const string SiteConfigKey = "SiteConfig";

        public static SiteConfig GetSiteConfig(HttpContextBase httpContext, IOptionService optionService)
        {
            var siteConfig = httpContext.CacheRead<SiteConfig>(SiteConfigKey);
            int durationMinutes =
                Convert.ToInt32(ConfigurationManager.AppSettings["CacheOptionsDuration"]);

            if (siteConfig == null)
            {
                siteConfig = optionService.GetAll();
                httpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);
            }
            return siteConfig;
        }

        public static void RemoveSiteConfig(HttpContextBase httpContext)
        {
            httpContext.InvalidateCache(SiteConfigKey);
        }
    }
}