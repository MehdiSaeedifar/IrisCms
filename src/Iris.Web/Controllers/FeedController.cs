using System.Collections.Generic;
using System.Linq;
using DNTCommon.Web.Core;
using Iris.Model.RSSModel;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class FeedController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public FeedController(IPostService postService, ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [ResponseCache(Duration = 3600)]
        public virtual ActionResult Posts()
        {
            var list = _postService.GetRssPosts(40);
            var feedItemsList = MapPostsToFeedItems(list);

            var channel = new FeedChannel
            {
                FeedTitle = "کتاب های EBooksWorld.ir",
                FeedDescription = "",
                FeedCopyright = "",
                FeedImageContentPath = "",
                FeedImageTitle = "",
                RssItems = feedItemsList,
                CultureName = "fa-IR"
            };

            return new FeedResult(channel);
        }

        [ResponseCache(Duration = 3600)]
        public virtual ActionResult Comments()
        {
            var list = _commentService.GetRssComments(20);
            var feedItemsList = MapCommentsToFeedItems(list);

            var channel = new FeedChannel
            {
                FeedTitle = "نظرات EBooksWorld.ir",
                FeedDescription = "",
                FeedCopyright = "",
                FeedImageContentPath = "",
                FeedImageTitle = "",
                RssItems = feedItemsList,
                CultureName = "fa-IR"
            };

            return new FeedResult(channel);
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
                Url = Url.Action("Index", "Post", new
                {
                    id = item.Id,
                    title = UrlExtensions.ResolveTitleForUrl(item.Title)
                }, HttpContext.Request.Scheme)
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
                Url = Url.Action("Index", "Post", new
                {
                    id = item.Id,
                    title = UrlExtensions.ResolveTitleForUrl(item.Title)
                }, HttpContext.Request.Scheme)
            }).ToList();
        }
    }
}
