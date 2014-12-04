using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Iris.Web.Controllers;
using StructureMap;

namespace Iris.Web.Infrastructure
{
    public class ViewConvertor : Controller, IViewConvertor
    {
        private readonly HttpContextBase _httpContext;
        public ViewConvertor(HttpContextBase httpContextBase)
        {
            _httpContext = httpContextBase;
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(CreateController<IrisController>().ControllerContext ,viewName);
                var viewContext = new ViewContext(CreateController<IrisController>().ControllerContext, viewResult.View,
                                             ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Creates an instance of an MVC controller from scratch 
        /// when no existing ControllerContext is present       
        /// </summary>
        /// <typeparam name="T">Type of the controller to create</typeparam>
        /// <returns>Controller Context for T</returns>
        /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
        public T CreateController<T>(RouteData routeData = null)
                    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (System.Web.HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

    }
}