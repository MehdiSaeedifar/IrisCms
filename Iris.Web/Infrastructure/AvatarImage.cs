using System.IO;
using System.Web;

namespace Iris.Web.Infrastructure
{
    public class AvatarImage
    {
        private static string _basePath;
        private static string _defaultPath;

        public static string BasePath
        {
            get { return _basePath; }
            set { _basePath = value; }
        }

        public static string DefaultPath
        {
            get { return _defaultPath; }
            set { _defaultPath = value; }
        }

        public static string GetAvatarImage(string userName)
        {
            string path = @HttpContext.Current.Server.MapPath(_basePath + userName + ".gif");
            if (File.Exists(path))
            {
                return string.Format("{0}/{1}.{2}", _basePath, userName, "gif");
            }
            return _defaultPath;
        }

        public static void RemoveAvatarImage(string userName)
        {
            string path = @HttpContext.Current.Server.MapPath(_basePath + userName + ".gif");
            File.Delete(path);
        }

        public static bool Exist(string userName)
        {
            string path = @HttpContext.Current.Server.MapPath(_basePath + userName + ".gif");
            return File.Exists(path);
        }
    }
}