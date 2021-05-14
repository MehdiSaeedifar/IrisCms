
using System.Linq;
using Iris.Servicelayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public HomeController(
            IPostService postService,
            IBookService bookService,
            ICategoryService categoryService
        )
        {
            _postService = postService;
            _bookService = bookService;
            _categoryService = categoryService;
        }

        public virtual ActionResult Index()
        {
            var booksList = _postService.GetBooksList(0, 16);
            return View(booksList);
        }

        [HttpPost]
        public virtual ActionResult BooksList([FromBody] GetBooksListModel model)
        {
            var booksList = _postService.GetBooksList(model.Page, model.Count);
            if (booksList == null || !booksList.Any())
                return Content("no-more");

            return PartialView("_BooksList", booksList);
        }

        public virtual ActionResult Slider()
        {
            return PartialView("_Slider", _bookService.GetSliderImages(6));
        }

        public virtual ActionResult NavBar()
        {
            return PartialView("_NavBar");
        }

        public virtual ActionResult NavbarItems()
        {
            return ViewComponent("NavbarItems");
        }

        public virtual ActionResult Announce()
        {
            return PartialView("_Announce", _categoryService.GetAnnounceData(5));
        }
    }

    public class NavbarItemsViewComponent : ViewComponent
    {
        private readonly IPageService _pageService;

        public NavbarItemsViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IViewComponentResult Invoke()
        {
            return View("_NavBarItems", _pageService.GetNavBarData(page => page.Status == "visible"));
        }
    }

    public class GetBooksListModel
    {
        public int Page { get; set; }
        public int Count { get; set; } = 16;
    }
}
