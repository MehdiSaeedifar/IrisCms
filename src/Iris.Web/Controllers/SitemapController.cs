using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DNTCommon.Web.Core;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IPostService _postService;
        private readonly IHtmlHelper _htmlHelper;

        public SitemapController(IPostService postService, IHtmlHelper htmlHelper)
        {
            _postService = postService;
            _htmlHelper = htmlHelper;
        }

        [Route("/sitemap.xml")]
        public IActionResult Index()
        {
            var siteMapItems = _postService.GetSiteMapData(50);

            var items = siteMapItems.Select(item => new SitemapItem
            {
                LastUpdatedTime = item.ModifiedDate ?? item.CreatedDate,
                Url = Url.Action(
                    "Index",
                    "Post",
                    new { id = item.Id, title = _htmlHelper.ResolveTitleForUrl(item.Title) },
                    HttpContext.Request.Scheme
                )
            }).ToList();

            return new SitemapResult(items);
        }
    }
}
