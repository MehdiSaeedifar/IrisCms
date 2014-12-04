using System.Collections.Generic;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Web.Filters;
using Iris.Web.Helpers;
using Iris.Web.HtmlCleaner;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    public partial class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ArticleController(IUnitOfWork uow, IArticleService articleService, ICategoryService categoryService,
            IUserService userService)
        {
            _uow = uow;
            _articleService = articleService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public virtual ActionResult Index()
        {
            return PartialView(MVC.Admin.Article.Views._Index);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            IList<Category> lstCategories = _categoryService.GetAll();
            ViewBag.CategoriesDropDownList = new SelectList(lstCategories, "Id", "Name");

            ViewBag.ArticleStatus = DropDownList.Status();
            ViewBag.CommentStatus = DropDownList.CommentStatus();

            return PartialView(MVC.Admin.Article.Views._Add);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Add(AddUpdateArticleModel model)
        {
            _articleService.Add(new Article
            {
                Body = model.Body.ToSafeHtml(),
                Category = _categoryService.Find(model.CategoryId),
                CommentStatus = model.CommentStatus,
                CreatedDate = DateAndTime.GetDateTime(),
                Description = model.Description,
                Keyword = model.Keywords,
                LikeCount = 0,
                Status = model.ArticleStatus,
                Title = model.Title,
                User = _userService.Find(User.Identity.Name),
                VisitedCount = 0,
            });

            _uow.SaveChanges();

            return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
            {
                Message = "مطلب جدید با موفقیت در سیستم ثبت شد",
                Mode = AlertMode.Success
            });
        }

        public virtual ActionResult DataTable(string term = "", int page = 0, int count = 10,
            Order order = Order.Descending, ArticleOrderBy orderBy = ArticleOrderBy.Date,
            ArticleSearchBy searchBy = ArticleSearchBy.Title)
        {
            ViewBag.CurrentPage = page;
            ViewBag.Count = count;
            ViewBag.TERM = term;
            ViewBag.PAGE = page;
            ViewBag.COUNT = count;
            ViewBag.ORDER = order;
            ViewBag.ORDERBY = orderBy;
            ViewBag.SEARCHBY = searchBy;

            ViewBag.OrderByList = DropDownList.OrderList(order);
            ViewBag.CountList = DropDownList.CountList(count);
            var selectListOrderBy = new List<SelectListItem>
            {
                new SelectListItem {Value = "Date", Text = "تاریخ ارسال"},
                new SelectListItem {Value = "Title", Text = "عنوان"},
                new SelectListItem {Value = "CommentCount", Text = "تعداد دیدگاه"},
                new SelectListItem {Value = "UserName", Text = "نام کاربری"},
                new SelectListItem {Value = "Order", Text = "ترتیب"}
            };

            ViewBag.OrderByItems = new SelectList(selectListOrderBy, "Value", "Text", orderBy);


            IList<ArticleDataTableModel> model = _articleService.GetDataTable(term, page, count, order, orderBy,
                searchBy);


            ViewBag.TotalRecords = (string.IsNullOrEmpty(term)) ? _articleService.Count : model.Count;


            return PartialView(MVC.Admin.Article.Views._DataTable, model);
        }

        [HttpGet]
        public virtual ActionResult ConfirmDelete(int id)
        {
            ViewBag.Id = id;
            return PartialView(MVC.Admin.Article.Views._ConfirmDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            _articleService.Remove(id);
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = "مطلب مورد نظر با موفقیت حذف شد", Mode = AlertMode.Success });
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            AddUpdateArticleModel selectedArticle = _articleService.GetUpdateData(id);

            IList<Category> lstCategories = _categoryService.GetAll();
            ViewBag.CategoriesDropDownList = new SelectList(lstCategories, "Id", "Name", selectedArticle.CategoryId);

            ViewBag.ArticleStatus = DropDownList.Status(selectedArticle.ArticleStatus);
            ViewBag.CommentStatus = DropDownList.CommentStatus(selectedArticle.CommentStatus);

            return PartialView(MVC.Admin.Article.Views._Edit, selectedArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(AddUpdateArticleModel model)
        {
            var selectedArticle = new Article
            {
                Body = model.Body.ToSafeHtml(),
                Category = _categoryService.Find(model.CategoryId),
                CommentStatus = model.CommentStatus,
                Description = model.Description,
                EditedByUser = _userService.Find(User.Identity.Name),
                Id = model.Id,
                Keyword = model.Keywords,
                ModifiedDate = DateAndTime.GetDateTime(),
                Status = model.ArticleStatus,
                Title = model.Title,
            };
            _articleService.Update(selectedArticle);
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = "مطلب مورد نظر با موفقیت ویرایش شد", Mode = AlertMode.Success });
        }
    }
}