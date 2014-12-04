using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.Model;
using Iris.Model.LuceneModel;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;
using Iris.Web.Searching;

namespace Iris.Web.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public PostController(IUnitOfWork uow, IPostService postSerivce, IUserService userService)
        {
            _uow = uow;
            _postService = postSerivce;
            _userService = userService;
        }

        public virtual ActionResult Index(int id)
        {
            _postService.IncrementVisitedNumber(id);
            _uow.SaveChanges();
            return View(_postService.GetPost(id));
        }

        [SiteAuthorize]
        [HttpPost]
        public virtual ActionResult Like(int id)
        {
            string result;
            int likeCount = 0;
            if (!_postService.IsUserLikePost(id, User.Identity.Name))
            {
                likeCount = _postService.Like(id, _userService.Find(User.Identity.Name));
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
            if (!_postService.IsUserLikePost(id, User.Identity.Name))
            {
                likeCount = _postService.DisLike(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }
            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount) });
        }

        [OutputCache(Duration = 60, VaryByParam = "id")]
        public virtual ActionResult SilmilarPosts(int id)
        {
            IEnumerable<MorePostsLikeThis> model = LuceneBookSearch.GetMoreLikeThisPostItems(id).Skip(1);
            return PartialView(MVC.Post.Views._SimilarPosts, model);
        }

        public virtual ActionResult UserPosts(string userName, int page = 0, int count = 100)
        {
            if (string.IsNullOrEmpty(userName)) userName = User.Identity.Name;

            IList<PostDetailModel> model = _postService.GetUserPosts(userName, page, count);
            ViewBag.UserName = userName;
            return PartialView(MVC.Post.Views._UserPosts, model);
        }

        public virtual ActionResult UserPostsList(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }
    }
}