using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;

namespace Iris.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.MapRouteLowercase("PostRoute", "Post/{action}/{id}/{title}", new
            {
                area = "",
                controller = "Post",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional
            }, new[] { "Iris.Web.Controllers" }
                );

            routes.MapRouteLowercase("PageRoute", "Page/{action}/{id}/{title}", new
            {
                area = "",
                controller = "Page",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional
            }, new[] { "Iris.Web.Controllers" }
                );

            routes.MapRouteLowercase("ArticleRoute", "Article/{action}/{id}/{title}", new
            {
                area = "",
                controller = "Article",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional
            }, new[] { "Iris.Web.Controllers" }
                );

            routes.MapRouteLowercase("LabelRoute", "Label/{action}/{id}/{title}/{name}", new
            {
                area = "",
                controller = "Label",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional,
                name = UrlParameter.Optional
            }, new[] { "Iris.Web.Controllers" }
                );

            routes.MapRouteLowercase("UserRoute", "User/{action}/{userName}", new
            {
                area = "",
                controller = "User",
                action = "Index",
                userName = UrlParameter.Optional
            }, new[] { "Iris.Web.Controllers" }
                );

            routes.MapRouteLowercase("Default", "{controller}/{action}/{id}", new
            {
                area = "",
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
            }, new[] { "Iris.Web.Controllers" }
                );
        }
    }
}