using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.Web.Helpers
{
    public static class UserAvatarHelper
    {
        public static string UserAvatarPath(this IHtmlHelper htmlHelper, string userName, ActionContext context)
        {
            return context.HttpContext.RequestServices.GetRequiredService<IAvatarImage>()
                .GetAvatarImage(userName);
        }
    }
}