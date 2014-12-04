using System.Web.Mvc;

namespace Iris.Web.Areas.Admin.Controllers
{
    public partial class FileController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult AvatarImage(string imageName)
        {
            return File(Server.MapPath("~/App_Data/UsersAvatars/" + imageName), "gif");
        }
    }
}