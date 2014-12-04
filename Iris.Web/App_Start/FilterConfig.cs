using System.Web.Mvc;
using Iris.Web.Infrastructure;

namespace Iris.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ForceWww());
        }
    }
}