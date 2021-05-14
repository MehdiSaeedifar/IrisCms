using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Iris.Web.Infrastructure
{
    public interface IAvatarImage
    {
        string GetAvatarImage(string userName);
        void RemoveAvatarImage(string userName);
        bool Exist(string userName);
    }

    public class AvatarImage : IAvatarImage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AvatarImage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetAvatarImage(string userName)
        {
            string path = $"/Content/avatars/{userName}.gif";
            if (!File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "Content", "avatars", userName + ".gif")))
            {
                return "/Content/Images/user.gif";
            }
            return path;
        }

        public void RemoveAvatarImage(string userName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "avatars", userName + ".gif");
            File.Delete(path);
        }

        public bool Exist(string userName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "avatars", userName + ".gif");
            return File.Exists(path);
        }
    }
}