using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Filters;
using Iris.Web.Helpers;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    public partial class LabelController : Controller
    {
        private readonly ILabelService _labelService;
        private readonly IUnitOfWork _uow;

        public LabelController(IUnitOfWork uow, ILabelService labelService)
        {
            _uow = uow;
            _labelService = labelService;
        }

        public virtual ActionResult Index()
        {
            return PartialView(MVC.Admin.Label.Views._Index);
        }

        public virtual ActionResult DataTable(string term = "", int page = 0, int count = 10,
            Order order = Order.Asscending, LabelOrderBy orderBy = LabelOrderBy.Id,
            LabelSearchBy searchBy = LabelSearchBy.Name)
        {
            ViewBag.CurrentPage = page;

            ViewBag.Count = count;

            ViewBag.TERM = term;
            ViewBag.PAGE = page;
            ViewBag.COUNT = count;
            ViewBag.ORDER = order;
            ViewBag.ORDERBY = orderBy;
            ViewBag.SEARCHBY = searchBy;

            var selectListOrderBy = new List<SelectListItem>
            {
                new SelectListItem {Text = "تاریخ", Value = "Id"},
                new SelectListItem {Text = "نام", Value = "Name"},
                new SelectListItem {Text = "توضیحات", Value = "Description"},
                new SelectListItem {Text = "تعداد پست ها", Value = "PostCount"}
            };


            ViewBag.OrderByItems = new SelectList(selectListOrderBy, "Value", "Text", orderBy);

            ViewBag.OrderByList = DropDownList.OrderList(order);

            ViewBag.CountList = DropDownList.CountList(count);

            IList<LabelDataTableModel> model = _labelService.GetAll(term, page, count, order, orderBy, searchBy);

            ViewBag.TotalRecords = (string.IsNullOrEmpty(term)) ? _labelService.Count : model.Count;

            return PartialView(MVC.Admin.Label.Views._DataTable, model);
        }

        public virtual ActionResult AutoCompleteSearch(string term)
        {
            var result = _labelService.GetLabels(label => label.Name.Contains(term))
                .Select(label => new { label = label.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            return PartialView(MVC.Admin.Label.Views._Add);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Add(AddLabelModel labelModel)
        {
            if (_labelService.IsExist(labelModel.Name))
            {
                return PartialView(MVC.Admin.Shared.Views._Alert,
                    new Alert { Message = "برچسبی با این نام موجود می باشد", Mode = AlertMode.Error });
            }
            _labelService.Add(new Label { Name = labelModel.Name, Description = labelModel.Description });
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert
                {
                    Message = "برچسب جدید با موفقیت در سیستم ثبت شد",
                    Mode = AlertMode.Success
                });
        }

        [HttpGet]
        public virtual ActionResult ConfirmDelete(int id)
        {
            ViewBag.LabelId = id;
            return PartialView(MVC.Admin.Label.Views._ConfirmDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            _labelService.Remove(id);
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = "برچسب مورد نظر با موفقیت حذف شد", Mode = AlertMode.Success });
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            Label selectedLabel = _labelService.GetLabel(id);

            return PartialView(MVC.Admin.Label.Views._Edit, new EditLabelModel
            {
                Id = selectedLabel.Id,
                Name = selectedLabel.Name,
                Description = selectedLabel.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditLabelModel labelModel)
        {
            _labelService.Update(new Label
            {
                Id = labelModel.Id,
                Name = labelModel.Name,
                Description = labelModel.Description
            });
            _uow.SaveChanges();
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = "برچسب مورد نظر با موفقیت ویرایش شد", Mode = AlertMode.Success });
        }
    }
}