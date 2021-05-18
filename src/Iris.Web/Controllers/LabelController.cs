using System.Linq;
using Iris.Servicelayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class LabelController : Controller
    {
        private readonly IPostService _postService;

        public LabelController(IPostService postService)
        {
            _postService = postService;
        }

        [Route("label/index/{id:int}/{title?}/{name?}")]
        public virtual ActionResult Index(int id, string name)
        {
            ViewBag.Id = id;
            ViewBag.Title = name;
            return View();
        }

        public virtual ActionResult BooksList(int labelId, [FromBody] BooksListModel model)
        {
            return ViewComponent("BooksList", new { labelId, model.Count, model.Page });
        }
    }

    public class BooksListModel
    {
        public int Page { get; set; }
        public int Count { get; set; } = 16;
    }

    public class BooksListViewComponent : ViewComponent
    {
        private readonly IPostService _postService;

        public BooksListViewComponent(IPostService postService)
        {
            _postService = postService;
        }

        public IViewComponentResult Invoke(int labelId, int page, int count = 16)
        {
            var labelsList = _postService.GetBooksList(labelId, page, count);

            if (!labelsList.Any())
                return Content("no-more");

            return View("_BooksList", labelsList);
        }
    }
}
