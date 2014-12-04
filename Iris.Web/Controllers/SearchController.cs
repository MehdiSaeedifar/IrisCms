using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Iris.Model.LuceneModel;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Helpers;
using Iris.Web.Searching;

namespace Iris.Web.Controllers
{
    public partial class SearchController : Controller
    {
        private readonly IBookService _bookService;

        public SearchController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public virtual ActionResult Index(string term)
        {
            ViewBag.Term = term;

            IEnumerable<LuceneBookModel> model = LuceneBookSearch.Search(term,
                "Name", "Description", "Name", "Title", "Author", "Publisher", "ISBN", "Description").Take(100);

            const string highlightPattern = @"<b style='color:red;'>$1</b>";

            foreach (LuceneBookModel book in model)
            {
                book.Name = Regex.
                    Replace(book.Name ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
                book.Description = Regex.
                    Replace(book.Description ?? " ", string.Format("({0})", term), highlightPattern,
                        RegexOptions.IgnoreCase);
                book.Author = Regex.
                    Replace(book.Author ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
                book.Publisher = Regex.
                    Replace(book.Publisher ?? " ", string.Format("({0})", term), highlightPattern,
                        RegexOptions.IgnoreCase);
                book.ISBN = Regex.
                    Replace(book.ISBN ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public virtual ActionResult AutoCompleteSearch(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Content(string.Empty);

            IEnumerable<LuceneBookModel> items =
                LuceneBookSearch.Search(term, "Name", "Title", "Author", "Publisher", "ISBN", "Description").Take(10);


            var data =
                items.Select(x => new
                {
                    label = x.Name,
                    url =
                        Url.Action(MVC.Post.ActionNames.Index, MVC.Post.Name,
                            new { id = x.PostId, title = UrlExtensions.ResolveTitleForUrl(x.Title) })
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}