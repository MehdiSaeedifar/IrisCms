using System;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Iris.Web.Caching
{
    public class CacheService : ICacheService
    {
        public const string SiteConfigKey = "SiteConfig";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionService _optionService;

        public CacheService(IHttpContextAccessor httpContextAccessor, IOptionService optionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _optionService = optionService;
        }

        public SiteConfig GetSiteConfig()
        {
            var siteConfig = _httpContextAccessor.HttpContext.CacheRead<SiteConfig>(SiteConfigKey);
            var durationMinutes =
                Convert.ToInt32(10);

            if (siteConfig != null) return siteConfig;

            siteConfig = _optionService.GetAll();
            _httpContextAccessor.HttpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);

            return siteConfig;
        }

        public void RemoveSiteConfig()
        {
            _httpContextAccessor.HttpContext.InvalidateCache(SiteConfigKey);
        }
    }
}