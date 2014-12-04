using System;
using System.Linq;
using System.Web;
using Castle.DynamicProxy;

namespace Iris.Utilities.Caching
{
    public class CacheInterceptor : IInterceptor
    {
        private static readonly object lockObject = new object();

        public void Intercept(IInvocation invocation)
        {
            cacheMethod(invocation);
        }

        private static void cacheMethod(IInvocation invocation)
        {
            var cacheMethodAttribute = getCacheMethodAttribute(invocation);
            if (cacheMethodAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            var cacheDuration = ((CacheMethodAttribute)cacheMethodAttribute).SecondsToCache;

            var cacheKey = getCacheKey(invocation);

            var cache = HttpRuntime.Cache;
            var cachedResult = cache.Get(cacheKey);


            if (cachedResult != null)
            {
                invocation.ReturnValue = cachedResult;
            }
            else
            {
                lock (lockObject)
                {
                    invocation.Proceed();
                    if (invocation.ReturnValue == null)
                        return;

                    cache.Insert(cacheKey, invocation.ReturnValue, null, DateTime.Now.AddSeconds(cacheDuration), TimeSpan.Zero);
                }
            }
        }

        private static Attribute getCacheMethodAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            return Attribute.GetCustomAttribute(methodInfo, typeof(CacheMethodAttribute), true);
        }

        private static string getCacheKey(IInvocation invocation)
        {

            var cacheKey = invocation.Method.Name + invocation.Method.GetHashCode();

            foreach (var argument in invocation.Arguments)
            {
                cacheKey += ":" + argument;
            }

            // todo: بهتر است هش اين كليد طولاني بازگشت داده شود
            // كار كردن با هش سريعتر خواهد بود
            return cacheKey;
        }
    }
}
