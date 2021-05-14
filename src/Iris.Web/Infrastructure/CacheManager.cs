using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.Web.Infrastructure
{
    public static class CacheManager
    {
        public static void CacheInsert(this HttpContext httpContext, string key, object data, int durationMinutes)
        {
            if (data == null) return;

            var cacheService =httpContext.RequestServices.GetRequiredService<IMemoryCache>();

            cacheService.CreateEntry(key).SetValue(data)
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(durationMinutes));

        }

        public static T CacheRead<T>(this HttpContext httpContext, string key)
        {
            var cacheService = httpContext.RequestServices.GetRequiredService<IMemoryCache>();

            cacheService.TryGetValue(key, out var value);

            return (T)value;
        }

        public static void InvalidateCache(this HttpContext httpContext, string key)
        {
            var cacheService = httpContext.RequestServices.GetRequiredService<IMemoryCache>();

            cacheService.Remove(key);
        }
    }
}