using System.Linq;
using Iris.Model.SideBar;
using Iris.Servicelayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class SideBarController : Controller
    {
        public virtual ActionResult Index()
        {
            return PartialView("_SideBar");
        }
    }

    public class SideBarCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public SideBarCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _categoryService.GetSideBarData();
            return View("_Categories", model);
        }
    }

    public class SideBarLabelsViewComponent : ViewComponent
    {
        private readonly ILabelService _labelService;

        public SideBarLabelsViewComponent(ILabelService labelService)
        {
            _labelService = labelService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _labelService.GetAll().OrderBy(label => label.Name);
            return View("_Labels", model);
        }
    }

    public class SideBarStatisticsViewComponent : ViewComponent
    {
        private readonly IBookService _bookService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public SideBarStatisticsViewComponent(
            IBookService bookService,
            ICommentService commentService,
            IUserService userService
        )
        {
            _bookService = bookService;
            _commentService = commentService;
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new SideBarModel
            {
                BookCount = _bookService.Count,
                CommentCount = _commentService.Count,
                UserCount = _userService.Count,
                ActiveUsersNumber = _userService.GetLastMonthActiveUsersCount()
            };
            return View("_Statistics", model);
        }
    }
}