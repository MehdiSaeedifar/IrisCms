using System.Collections.Generic;
using Iris.Datalayer.Context;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class ArticleController : Controller
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
            var model = _articleService.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            _articleService.IncrementVisitedCount(id);
            _uow.SaveChanges();
            return View(model);
        }

        [SiteAuthorize]
        [HttpPost("article/like/{id}")]

        public virtual ActionResult Like([FromRoute] int id)
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
        [HttpPost("article/dislike/{id}")]
        public virtual ActionResult DisLike([FromRoute] int id)
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
            return PartialView("_UserArticles", model);
        }

        public virtual ActionResult UserArticlesList(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }
    }
}
