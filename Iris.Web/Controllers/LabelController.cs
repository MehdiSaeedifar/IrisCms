using System.Linq;
using System.Web.Mvc;
using Iris.Model;
using Iris.Servicelayer.Interfaces;

namespace Iris.Web.Controllers
{
    public partial class LabelController : Controller
    {
        private readonly IPostService _postService;

        public LabelController(IPostService postService)
        {
            _postService = postService;
        }

        public virtual ActionResult Index(int id, string name)
        {
            ViewBag.Id = id;
            ViewBag.Title = name;
            return View();
        }

        public virtual ActionResult BooksList(int labelId, int page = 0, int count = 8)
        {
            IOrderedEnumerable<BooksListModel> labelsList =
                _postService.GetBooksList(labelId, page, count).OrderByDescending(post => post.CreatedDate);
            if (!labelsList.Any())
                return Content("no-more");

            return PartialView(MVC.Label.Views._BooksList, labelsList);
        }
    }
}