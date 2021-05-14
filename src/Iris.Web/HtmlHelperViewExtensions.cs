using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.Web
{
    public static class HtmlHelperViewExtensions
    {
        public static IHtmlContent Action(this IHtmlHelper helper, string action, object parameters = null)
        {
            var controller = (string)helper.ViewContext.RouteData.Values["controller"];
            return Action(helper, action, controller, parameters);
        }

        public static IHtmlContent Action(this IHtmlHelper helper, string action, string controller, object parameters = null)
        {
            var area = (string)helper.ViewContext.RouteData.Values["area"];
            return Action(helper, action, controller, area, parameters);
        }

        public static IHtmlContent Action(this IHtmlHelper helper, string action, string controller, string area, object parameters = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(controller));
            if (controller == null)
                throw new ArgumentNullException(nameof(action));

            var task = ActionAsync(helper, action, controller, area, parameters);
            return task.Result;
        }

        private static async Task<IHtmlContent> ActionAsync(this IHtmlHelper helper, string action, string controller, string area, object parameters = null)
        {
            // fetching required services for invocation
            var currentHttpContext = helper.ViewContext.HttpContext;
            var httpContextFactory = GetServiceOrFail<IHttpContextFactory>(currentHttpContext);
            var actionInvokerFactory = GetServiceOrFail<IActionInvokerFactory>(currentHttpContext);
            var actionSelector = GetServiceOrFail<IActionDescriptorCollectionProvider>(currentHttpContext);

            // creating new action invocation context
            var routeData = new RouteData();
            var routeParams = new RouteValueDictionary(parameters ?? new { });
            var routeValues = new RouteValueDictionary(new { area, controller, action });
            var newHttpContext = httpContextFactory.Create(currentHttpContext.Features);

            newHttpContext.Response.Body = new MemoryStream();

            foreach (var router in helper.ViewContext.RouteData.Routers)
                routeData.PushState(router, null, null);

            routeData.PushState(null, routeValues, null);
            routeData.PushState(null, routeParams, null);

            var actionDescriptor = actionSelector.ActionDescriptors.Items.First(i => i.RouteValues["Controller"] == controller && i.RouteValues["Action"] == action);
            var actionContext = new ActionContext(newHttpContext, routeData, actionDescriptor);

            // invoke action and retreive the response body
            var invoker = actionInvokerFactory.CreateInvoker(actionContext);
            string content = null;

            await invoker.InvokeAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    content = task.Exception.Message;
                }
                else if (task.IsCompleted)
                {
                    newHttpContext.Response.Body.Position = 0;
                    var reader = new StreamReader(newHttpContext.Response.Body);
                        content = reader.ReadToEnd();
                }
            });

            return new HtmlString(content);
        }

        private static TService GetServiceOrFail<TService>(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var service = httpContext.RequestServices.GetService(typeof(TService));

            if (service == null)
                throw new InvalidOperationException($"Could not locate service: {nameof(TService)}");

            return (TService)service;
        }

    }
}
