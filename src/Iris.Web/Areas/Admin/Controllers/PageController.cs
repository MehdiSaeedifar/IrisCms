using System.Collections.Generic;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Web.Filters;
using Iris.Web.Helpers;
using Iris.Web.HtmlCleaner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    [Area("Admin")]
    public class PageController : Controller
    {
        private readonly IPageService _pageSerivce;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public PageController(IUnitOfWork uow, IPageService pageService, IUserService userService)
        {
            _uow = uow;
            _pageSerivce = pageService;
            _userService = userService;
        }

        public virtual ActionResult Index()
        {
            return PartialView("_Index");
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            IList<PageDropDownList> lstPages = _pageSerivce.DropDownListData();
            lstPages.Insert(0, new PageDropDownList { Id = -1, Title = "بدون مادر" });
            ViewBag.ParentPagesList = new SelectList(lstPages, "Id", "Title");

            ViewBag.PageStatus = DropDownList.Status();
            ViewBag.CommentStatus = DropDownList.CommentStatus();

            return PartialView("_Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Add(AddPageModel pageModel)
        {
            var newPage = new Page
            {
                Body = pageModel.Body.ToSafeHtml(),
                CommentStatus = pageModel.CommentStatus,
                CreatedDate = DateAndTime.GetDateTime(),
                Keyword = pageModel.Keyword,
                Order = pageModel.Order,
                Parent = _pageSerivce.Find(pageModel.ParentId.Value),
                Status = pageModel.Status,
                Title = pageModel.Title,
                User = _userService.Find(User.Identity.Name),
                Description = pageModel.Description
            };
            _pageSerivce.Add(newPage);
            _uow.SaveChanges();

            return PartialView("_Alert", new Alert
            {
                Message = "برگه جدید با موفقیت در سیستم ثبت شد",
                Mode = AlertMode.Success
            });
        }

        public virtual ActionResult DataTable(string term = "", int page = 0, int count = 10,
            Order order = Order.Descending, PageOrderBy orderBy = PageOrderBy.ParentTitle,
            PageSearchBy searchBy = PageSearchBy.Title)
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
                new SelectListItem {Value = "ParentTitle", Text = "مادر"}
            };

            ViewBag.OrderByItems = new SelectList(selectListOrderBy, "Value", "Text", orderBy);


            IList<PageDataTableModel> model = _pageSerivce.GetDataTable(term, page, count, order, orderBy, searchBy);


            ViewBag.TotalRecords = (string.IsNullOrEmpty(term)) ? _pageSerivce.Count : model.Count;


            return PartialView("_DataTable", model);
        }

        [HttpGet]
        public virtual ActionResult ConfirmDelete(int id)
        {
            ViewBag.Id = id;
            return PartialView("_ConfirmDelete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            _pageSerivce.Remove(id);
            _uow.SaveChanges();
            return PartialView("_Alert",
                new Alert { Message = "برگه مورد نظر با موفقیت حذف شد", Mode = AlertMode.Success });
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            EditPageModel selectedPage = _pageSerivce.GetEditingData(id);

            IList<PageDropDownList> lstPages = _pageSerivce.DropDownListData(selectedPage.Id);

            lstPages.Insert(0, new PageDropDownList { Id = -1, Title = "بدون مادر" });
            ViewBag.ParentPagesList = new SelectList(lstPages, "Id", "Title", selectedPage.ParentId);

            ViewBag.PageStatus = DropDownList.Status(selectedPage.Status);
            ViewBag.CommentStatus = DropDownList.CommentStatus(selectedPage.CommentStatus);

            return PartialView("_Edit", selectedPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditPageModel pageModel)
        {
            var selectedPage = new Page
            {
                Body = pageModel.Body.ToSafeHtml(),
                CommentStatus = pageModel.CommentStatus,
                Description = pageModel.Description,
                EditedByUser = _userService.Find(User.Identity.Name),
                Keyword = pageModel.Keyword,
                ModifiedDate = DateAndTime.GetDateTime(),
                Id = pageModel.Id,
                Order = pageModel.Order,
                Parent = _pageSerivce.Find(pageModel.ParentId.Value),
                Status = pageModel.Status,
                Title = pageModel.Title
            };
            _pageSerivce.Update(selectedPage);
            _uow.SaveChanges();
            return PartialView("_Alert",
                new Alert { Message = "برگه مورد نظر با موفقیت ویرایش شد", Mode = AlertMode.Success });
        }
    }
}