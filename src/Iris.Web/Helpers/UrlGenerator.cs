using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Helpers
{
    public static class UrlGenerator
    {
        public static HtmlString ReturnUrl(this IHtmlHelper htmlHelper, HttpContext contextBase,
            IUrlHelper urlHelper)
        {
            string currentUrl = contextBase.Request.GetEncodedUrl();
            if (currentUrl == "/")
            {
                currentUrl = urlHelper.Action("Index", "Home");
            }
            return new(currentUrl);
        }
    }
}