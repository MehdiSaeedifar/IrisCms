using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Helpers
{
    public static class UrlExtensions
    {
        public static string ResolveTitleForUrl(this IHtmlHelper htmlHelper, string title)
        {
            return string.IsNullOrEmpty(title)
                ? string.Empty
                : Regex.Replace(Regex.Replace(title, "[^\\w]", "-"), "[-]{2,}", "-");
        }

        public static string ResolveTitleForUrl(string title)
        {
            return string.IsNullOrEmpty(title)
                ? string.Empty
                : Regex.Replace(Regex.Replace(title, "[^\\w]", "-"), "[-]{2,}", "-");
        }
    }
}