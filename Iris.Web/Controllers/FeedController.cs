using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Iris.Model.RSSModel;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Helpers;
using Iris.Web.RSS;

namespace Iris.Web.Controllers
{
    public partial class FeedController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public FeedController(IPostService postService, ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [OutputCache(Duration = 3600)]
        public virtual ActionResult Posts()
        {
            IList<RssPostModel> list = _postService.GetRssPosts(20);
            IEnumerable<FeedItem> feedItemsList = MapPostsToFeedItems(list);
            return new FeedResult("فید کتاب های سایت", feedItemsList);
        }

        [OutputCache(Duration = 3600)]
        public virtual ActionResult Comments()
        {
            IList<RssCommentModel> list = _commentService.GetRssComments(20);
            IEnumerable<FeedItem> feedItemsList = MapCommentsToFeedItems(list);
            return new FeedResult("فید دیدگاه های مطالب", feedItemsList);
        }

        private IEnumerable<FeedItem> MapPostsToFeedItems(IEnumerable<RssPostModel> list)
        {
            return list.Select(item => new FeedItem
            {
                AuthorName = item.Author,
                Content = item.Body,
                LastUpdatedTime = item.UpdateDate ?? item.CreatedDate,
                PublishDate = item.CreatedDate,
                Title = item.Title,
                Url = Url.Action(MVC.Post.ActionNames.Index, MVC.Post.Name, new
                {
                    id = item.Id,
                    title = UrlExtensions.ResolveTitleForUrl(item.Title)
                }, "http")
            }).ToList();
        }

        private IEnumerable<FeedItem> MapCommentsToFeedItems(IEnumerable<RssCommentModel> list)
        {
            return list.Select(item => new FeedItem
            {
                AuthorName = item.Author,
                Content = item.Body,
                LastUpdatedTime = item.UpdateDate ?? item.CreatedDate,
                PublishDate = item.CreatedDate,
                Title = item.Title,
                Url = Url.Action(MVC.Post.ActionNames.Index, MVC.Post.Name, new
                {
                    id = item.Id,
                    title = UrlExtensions.ResolveTitleForUrl(item.Title)
                }, "http")
            }).ToList();
        }
    }
}