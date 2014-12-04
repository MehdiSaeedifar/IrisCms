using System.Collections.Generic;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Filters;
using Iris.Web.Helpers;
using Iris.Web.HtmlCleaner;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    public partial class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUnitOfWork _uow;

        public CommentController(IUnitOfWork uow, ICommentService commentService)
        {
            _uow = uow;
            _commentService = commentService;
        }

        public virtual ActionResult Index()
        {
            return PartialView(MVC.Admin.Comment.Views._Index);
        }

        public virtual ActionResult DataTable(string term = "", int page = 0, int count = 10,
            Order order = Order.Descending, CommentOrderBy orderBy = CommentOrderBy.AddDate,
            CommentSearchBy searchBy = CommentSearchBy.UserName)
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
                new SelectListItem {Value = "AddDate", Text = "تاریخ ارسال"},
                new SelectListItem {Value = "IsApproved", Text = "وضعیت"},
                new SelectListItem {Value = "LikeCount", Text = "رای ها"},
                new SelectListItem {Value = "UserName", Text = "نام کاربری"},
                new SelectListItem {Value = "Author", Text = "نویسنده"},
                new SelectListItem {Value = "IP", Text = "IP"}
            };

            ViewBag.OrderByItems = new SelectList(selectListOrderBy, "Value", "Text", orderBy);


            IList<CommentDataTableModel> model = _commentService.GetDataTable(term, page, count, order, orderBy,
                searchBy);


            ViewBag.TotalRecords = (string.IsNullOrEmpty(term)) ? _commentService.Count : model.Count;


            foreach (CommentDataTableModel item in model)
            {
                item.AvatarPath = Url.Content("~/Content/Images/user.gif");
            }

            return PartialView(MVC.Admin.Comment.Views._DataTable, model);
        }

        [HttpGet]
        public virtual ActionResult ConfirmDelete(int id)
        {
            ViewBag.Id = id;
            return PartialView(MVC.Admin.Comment.Views._ConfirmDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            _commentService.Remove(id);
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
            {
                Message = "دیدگاه مورد نظر با موفقیت حذف شد",
                Mode = AlertMode.Success
            });
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            Comment selectedComment = _commentService.GetComment(id);
            return PartialView(MVC.Admin.Comment.Views._Edit, new EditCommentModel
            {
                Id = selectedComment.Id,
                Body = selectedComment.Body
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditCommentModel commentModel)
        {
            _commentService.Update(new Comment { Id = commentModel.Id, Body = commentModel.Body.ToSafeHtml() });
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
            {
                Message = "دیدگاه مورد نظر با موفقیت به روز رسانی شد",
                Mode = AlertMode.Success
            });
        }

        [HttpGet]
        public virtual ActionResult Approve(int id)
        {
            _commentService.Approve(id);
            _uow.SaveChanges();
            return RenderApproveButtons(id, "disapproved");
        }

        [HttpGet]
        public virtual ActionResult DisApprove(int id)
        {
            _commentService.DisApprove(id);
            _uow.SaveChanges();
            return RenderApproveButtons(id, "approved");
        }

        [ChildActionOnly]
        public virtual ActionResult RenderApproveButtons(int id, string type)
        {
            ViewBag.Id = id;
            ViewBag.Status = (type == "approved") ? "approved" : "disapproved";
            return PartialView(MVC.Admin.Comment.Views._ApproveStatusButton);
        }

        public virtual ActionResult AutoCompleteSearch(string term, CommentSearchBy searchBy = CommentSearchBy.Body)
        {
            return new EmptyResult();
        }
    }
}