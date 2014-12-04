using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using MvcSiteMapProvider.Web;

namespace Iris.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IPageService _pageService;
        private readonly IPostService _postService;

        public HomeController(IPostService postService, IBookService bookService, IPageService pageService,
            ICategoryService categoryService)
        {
            _postService = postService;
            _bookService = bookService;
            _pageService = pageService;
            _categoryService = categoryService;
        }

        public virtual ActionResult Index()
        {
            IList<BooksListModel> booksList = _postService.GetBooksList(0, 8);
            return View(booksList);
        }

        [HttpPost]
        public virtual ActionResult BooksList(int page = 0, int count = 8)
        {
            IList<BooksListModel> booksList = _postService.GetBooksList(page, count);
            if (booksList == null || !booksList.Any())
                return Content("no-more");

            return PartialView(MVC.Home.Views._BooksList, booksList);
        }

        [OutputCache(Duration = 1200)]
        public virtual ActionResult Slider()
        {
            return PartialView(MVC.Home.Views._Slider, _bookService.GetSliderImages(6));
        }

        public virtual ActionResult NavBar()
        {
            return PartialView(MVC.Home.Views._NavBar);
        }

        [OutputCache(Duration = 900)]
        public virtual ActionResult NavbarItems()
        {
            return PartialView(MVC.Home.Views._NavBarItems, _pageService.GetNavBarData(page => page.Status == "visible"));
        }

        [OutputCache(Duration = 900)]
        public virtual ActionResult Announce()
        {
            return PartialView(MVC.Home.Views._Announce, _categoryService.GetAnnounceData(3));
        }

        public virtual ActionResult SiteMapXml()
        {
            return new XmlSiteMapResult();
        }
    }
}