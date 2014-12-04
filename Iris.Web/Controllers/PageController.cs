using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;

namespace Iris.Web.Controllers
{
    public partial class PageController : Controller
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
            Page model = _pageService.Find(id);
            _pageService.IncrementVisitedCount(id);
            _uow.SaveChanges();
            return View(model);
        }

        [SiteAuthorize]
        [HttpPost]
        public virtual ActionResult Like(int id)
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
        [HttpPost]
        public virtual ActionResult DisLike(int id)
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