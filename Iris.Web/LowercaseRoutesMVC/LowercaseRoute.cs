namespace LowercaseRoutesMVC
{
   using System.Web.Routing;

   internal class LowercaseRoute : Route
   {
      public LowercaseRoute(string url, IRouteHandler routeHandler)
         : base(url, routeHandler)
      {
      }

      public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
         : base(url, defaults, routeHandler)
      {
      }

      public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
         : base(url, defaults, constraints, routeHandler)
      {
      }

      public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
         : base(url, defaults, constraints, dataTokens, routeHandler)
      {
      }

      public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
      {
         VirtualPathData path = base.GetVirtualPath(requestContext, values);

         if (path != null)
         {
            string virtualPath = path.VirtualPath;
            var lastIndexOf = virtualPath.LastIndexOf("?");

            if (lastIndexOf != 0)
            {
               if (lastIndexOf > 0)
               {
                  string leftPart = virtualPath.Substring(0, lastIndexOf).ToLowerInvariant();
                  string queryPart = virtualPath.Substring(lastIndexOf);
                  path.VirtualPath = leftPart + queryPart;
               }
               else
               {
                  path.VirtualPath = path.VirtualPath.ToLowerInvariant();
               }
            }            
         }

         return path;
      }
   }
}