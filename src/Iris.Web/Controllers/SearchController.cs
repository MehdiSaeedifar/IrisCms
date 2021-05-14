using System.Collections.Generic;
using System.Linq;
using AspNetCore.Unobtrusive.Ajax;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBookSearch _bookSearch;

        public SearchController(IBookSearch bookSearch)
        {
            _bookSearch = bookSearch;
        }

        public virtual ActionResult Index(string term)
        {
            ViewBag.Term = term;

            IEnumerable<LuceneBookModel> model = _bookSearch.Search(term).Take(50);

            //const string highlightPattern = @"<b style='color:red;'>$1</b>";

            //foreach (LuceneBookModel book in model)
            //{
            //    book.Name = Regex.
            //        Replace(book.Name ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
            //    book.Description = Regex.
            //        Replace(book.Description ?? " ", string.Format("({0})", term), highlightPattern,
            //            RegexOptions.IgnoreCase);
            //    book.Author = Regex.
            //        Replace(book.Author ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
            //    book.Publisher = Regex.
            //        Replace(book.Publisher ?? " ", string.Format("({0})", term), highlightPattern,
            //            RegexOptions.IgnoreCase);
            //    book.ISBN = Regex.
            //        Replace(book.ISBN ?? " ", string.Format("({0})", term), highlightPattern, RegexOptions.IgnoreCase);
            //}
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

            var items =
                _bookSearch.AutoCompleteSearch(term).Take(20);


            var data =
                items.Select(x => new
                {
                    label = x.Name,
                    url =
                        Url.Action("Index", "Post",
                            new { id = x.PostId, title = UrlExtensions.ResolveTitleForUrl(x.Title) })
                });

            return Json(data);
        }
    }
}