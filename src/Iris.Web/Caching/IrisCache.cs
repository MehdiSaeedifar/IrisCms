using System;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Iris.Web.Caching
{
    public class IrisCache
    {
        public const string SiteConfigKey = "SiteConfig";

        public static SiteConfig GetSiteConfig(HttpContext httpContext, IOptionService optionService)
        {
            var siteConfig = httpContext.CacheRead<SiteConfig>(SiteConfigKey);
            int durationMinutes =
                Convert.ToInt32(10);

            if (siteConfig == null)
            {
                siteConfig = optionService.GetAll();
                httpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);
            }
            return siteConfig;
        }

        public static void RemoveSiteConfig(HttpContext httpContext)
        {
            httpContext.InvalidateCache(SiteConfigKey);
        }
    }
}