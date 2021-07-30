using Iris.Datalayer.Context;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public PageController(IUnitOfWork uow, IUserService userService, IPageService pageService)
        {
            _uow = uow;
            _userService = userService;
            _pageService = pageService;
        }

        public virtual ActionResult Index(int id)
        {
            var model = _pageService.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            _pageService.IncrementVisitedCount(id);

            return View(model);
        }

        [SiteAuthorize]
        [HttpPost("page/like/{id}")]
        public virtual ActionResult Like([FromRoute] int id)
        {
            string result;
            int likeCount = 0;
            if (!_pageService.IsUserLikePage(id, User.Identity.Name))
            {
                likeCount = _pageService.Like(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }

            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount) });
        }

        [SiteAuthorize]
        [HttpPost("page/dislike/{id}")]
        public virtual ActionResult DisLike([FromRoute] int id)
        {
            string result;
            int likeCount = 0;
            if (!_pageService.IsUserLikePage(id, User.Identity.Name))
            {
                likeCount = _pageService.DisLike(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }

            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount) });
        }
    }
}
