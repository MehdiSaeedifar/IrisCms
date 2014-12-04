using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Helpers
{
    public static class UserAvatarHelper
    {
        public static string UserAvatarPath(this HtmlHelper htmlHelper, string userName, HttpContextBase context)
        {
            string basePath = UrlHelper.GenerateContentUrl("~/Content/avatars", context);
            string defaultPath = UrlHelper.GenerateContentUrl("~/Content/Images", context);
            string path = @HttpContext.Current.Server.MapPath(string.Format("{0}/{1}.gif", basePath, userName));
            return File.Exists(path)
                ? string.Format("{0}/{1}.{2}", basePath, userName, "gif")
                : string.Format("{0}/{1}.{2}", defaultPath, "user", "gif");
        }
    }
}