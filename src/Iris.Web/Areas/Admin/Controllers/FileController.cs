using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        //public virtual ActionResult Index()
        //{
        //    return View();
        //}

        //public virtual ActionResult AvatarImage(string imageName)
        //{
        //    return File(Server.MapPath("~/App_Data/UsersAvatars/" + imageName), "gif");
        //}
    }
}