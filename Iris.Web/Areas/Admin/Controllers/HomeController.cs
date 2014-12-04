using System.Web.Mvc;
using Iris.Web.Infrastructure;

namespace Iris.Web.Areas.Admin.Controllers
{
    public partial class HomeController : Controller
    {
        [Authorize(Roles = "admin,moderator,writer,editor")]
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult RenderDashBoard()
        {
            // set avatar images for users
            AvatarImage.DefaultPath = Url.Content("~/Content/Images/user.gif");
            AvatarImage.BasePath = Url.Content("~/Content/avatars/");
            ViewBag.AvatarPath = AvatarImage.GetAvatarImage(User.Identity.Name);
            return PartialView(MVC.Admin.Home.Views._DashBoard);
        }
    }
}