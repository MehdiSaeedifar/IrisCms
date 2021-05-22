using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IBookSearch _bookSearch;

        public PostController(
            IUnitOfWork uow,
            IPostService postService,
            IUserService userService,
            IBookSearch bookSearch
        )
        {
            _uow = uow;
            _postService = postService;
            _userService = userService;
            _bookSearch = bookSearch;
        }

        [Route("post/index/{id:int}/{title?}")]
        public virtual ActionResult Index(int id)
        {
            var post = _postService.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            _postService.IncrementVisitedNumber(id);
            _uow.SaveChanges();

            return View(post);
        }

        [SiteAuthorize]
        [HttpPost("post/like/{id}")]
        public virtual ActionResult Like([FromRoute] int id)
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
        [HttpPost("post/dislike/{id}")]
        public virtual ActionResult DisLike([FromRoute] int id)
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

        [HttpPost("post/links")]
        public async Task<ActionResult> GetDownloadLinks(
            [FromBody] GetDownloadLinksModel model,
            CancellationToken cancellationToken
        )
        {
            if (model.Answer.Trim().ApplyCorrectYeKe() != "طهران")
            {
                return NotFound();
            }

            return PartialView("_DownloadLinks", await _postService.GetDownloadLinks(model.PostId, cancellationToken));
        }

        public virtual ActionResult SilmilarPosts(int id)
        {
            IEnumerable<MorePostsLikeThis> model = _bookSearch.GetMoreLikeThisPostItems(id);
            return PartialView("_SimilarPosts", model);
        }

        public virtual ActionResult UserPosts(string userName, int page = 0, int count = 100)
        {
            if (string.IsNullOrEmpty(userName)) userName = User.Identity.Name;

            IList<PostDetailModel> model = _postService.GetUserPosts(userName, page, count);
            ViewBag.UserName = userName;
            return PartialView("_UserPosts", model);
        }

        public virtual ActionResult UserPostsList(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }
    }

    public class GetDownloadLinksModel
    {
        public int PostId { get; set; }
        public string Answer { get; set; }
    }
}
