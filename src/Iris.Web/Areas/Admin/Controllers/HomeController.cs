using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAvatarImage _avatarImage;

        public HomeController(IAvatarImage avatarImage)
        {
            _avatarImage = avatarImage;
        }

        [Authorize(Roles = "admin,moderator,writer,editor")]
        [Area("Admin")]
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult RenderDashBoard()
        {
            // set avatar images for users
            ViewBag.AvatarPath = _avatarImage.GetAvatarImage(User.Identity.Name);
            return PartialView("_DashBoard");
        }
    }
}