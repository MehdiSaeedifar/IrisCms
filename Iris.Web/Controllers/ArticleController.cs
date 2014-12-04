using System.Collections.Generic;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;

namespace Iris.Web.Controllers
{
    public partial class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ArticleController(IUnitOfWork uow, IUserService userService, IArticleService articleService)
        {
            _uow = uow;
            _userService = userService;
            _articleService = articleService;
        }

        public virtual ActionResult Index(int id)
        {
            Article model = _articleService.Find(id);
            _articleService.IncrementVisitedCount(id);
            _uow.SaveChanges();
            return View(model);
        }

        [SiteAuthorize]
        [HttpPost]
        public virtual ActionResult Like(int id)
        {
            string result;
            int likeCount = 0;
            if (!_articleService.IsUserLikeArticle(id, User.Identity.Name))
            {
                likeCount = _articleService.Like(id, _userService.Find(User.Identity.Name));
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
            if (!_articleService.IsUserLikeArticle(id, User.Identity.Name))
            {
                likeCount = _articleService.DisLike(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }
            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount) });
        }

        public virtual ActionResult UserArticles(string userName, int page = 0, int count = 100)
        {
            if (string.IsNullOrEmpty(userName)) userName = User.Identity.Name;
            IList<PostDetailModel> model = _articleService.GetUserArticles(userName, page, count);
            ViewBag.UserName = userName;
            return PartialView(MVC.Article.Views._UserArticles, model);
        }

        public virtual ActionResult UserArticlesList(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }
    }
}