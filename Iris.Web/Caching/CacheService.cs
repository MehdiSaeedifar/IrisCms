using System;
using System.Configuration;
using System.Web;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Infrastructure;

namespace Iris.Web.Caching
{
    public class CacheService : ICacheService
    {
        public const string SiteConfigKey = "SiteConfig";

        private readonly HttpContextBase _httpContext;
        private readonly IOptionService _optionService;

        public CacheService(HttpContextBase httpContext, IOptionService optionService)
        {
            _httpContext = httpContext;
            _optionService = optionService;
        }

        public SiteConfig GetSiteConfig()
        {
            var siteConfig = _httpContext.CacheRead<SiteConfig>(SiteConfigKey);
            var durationMinutes =
                Convert.ToInt32(ConfigurationManager.AppSettings["CacheOptionsDuration"]);

            if (siteConfig != null) return siteConfig;

            siteConfig = _optionService.GetAll();
            _httpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);

            return siteConfig;
        }

        public void RemoveSiteConfig()
        {
            _httpContext.InvalidateCache(SiteConfigKey);
        }

    }
}