using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Utilities.Security;
using Iris.Web.Filters;
using Iris.Web.Helpers;
using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    [Area("Admin")]
    public partial class UserController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAvatarImage _avatarImage;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public UserController(
            IUnitOfWork uow,
            IUserService userService,
            IRoleService roleService,
            IAvatarImage avatarImage
        )
        {
            _uow = uow;
            _userService = userService;
            _roleService = roleService;
            _avatarImage = avatarImage;
        }

        public virtual ActionResult Index()
        {
            return PartialView("_Index");
        }

        public virtual ActionResult DataTable(string term = "", int page = 0, int count = 10,
            Order order = Order.Descending, UserOrderBy orderBy = UserOrderBy.RegisterDate,
            UserSearchBy searchBy = UserSearchBy.UserName)
        {
            ViewBag.TERM = term;
            ViewBag.PAGE = page;
            ViewBag.COUNT = count;
            ViewBag.ORDER = order;
            ViewBag.ORDERBY = orderBy;
            ViewBag.SEARCHBY = searchBy;


            IList<UserDataTableModel> selectedUsers = _userService.GetDataTable(term, page, count, order, orderBy,
                searchBy);


            ViewBag.OrderByList = DropDownList.OrderList(order);
            ViewBag.CountList = DropDownList.CountList(count);

            var selectListOrderBy = new List<SelectListItem>
            {
                new SelectListItem {Value = "RegisterDate", Text = "تاریخ ثبت نام"},
                new SelectListItem {Value = "UserName", Text = "نام کاربری"},
                new SelectListItem {Value = "CommentCount", Text = "تعداد دیدگاه"},
                new SelectListItem {Value = "PostCount", Text = "تعداد پست"},
                new SelectListItem {Value = "IsBaned", Text = "وضعیت"},
                new SelectListItem {Value = "LoginDate", Text = "تاریخ ورود"},
                new SelectListItem {Value = "IP", Text = "IP"}
            };

            ViewBag.OrderByItems = new SelectList(selectListOrderBy, "Value", "Text", orderBy);

            ViewBag.TotalRecords = (string.IsNullOrEmpty(term)) ? _userService.Count : selectedUsers.Count;

            // set avatar images for users
            foreach (UserDataTableModel user in selectedUsers)
            {
                user.AvatarPath = _avatarImage.GetAvatarImage(user.UserName);
            }

            return PartialView("_DataTable", selectedUsers);
        }

        public virtual ActionResult Detail(int userId)
        {
            UserDetailModel selectedUser = _userService.GetUserDetail(userId);


            selectedUser.AvatarPath = _avatarImage.GetAvatarImage(selectedUser.UserName);

            return PartialView("_Detail", selectedUser);
        }

        [HttpGet]
        public virtual ActionResult Search()
        {
            return PartialView("_Search");
        }

        //[HttpPost]
        //public virtual ActionResult Search(string term, string by, int id)
        //{
        //    throw new NotImplementedException();
        //}
        [HttpGet]
        public virtual ActionResult AutoCompleteSearch(string term, UserSearchBy searchBy = UserSearchBy.UserName)
        {
            IList<string> data = new List<string>();

            switch (searchBy)
            {
                case UserSearchBy.UserName:
                    data = _userService.SearchByUserName(term);
                    break;
                case UserSearchBy.FirstName:
                    data = _userService.SearchByFirstName(term);
                    break;
                case UserSearchBy.LastName:
                    data = _userService.SearchByLastName(term);
                    break;
                case UserSearchBy.Email:
                    data = _userService.SearchByEmail(term);
                    break;
                case UserSearchBy.Ip:
                    data = _userService.SearchByIP(term);
                    break;
                case UserSearchBy.RoleDescription:
                    data = _userService.SearchByRoleDescription(term);
                    break;
            }

            return Json(data.Select(x => new { label = x }).ToList());
        }

        [HttpGet]
        public virtual ActionResult RenderNavBar()
        {
            return PartialView("_NavBar");
        }

        #region Add User

        [HttpGet]
        public virtual ActionResult Add()
        {
            IEnumerable<SelectListItem> lst = _roleService.GetAllRoles().Select(x =>
                new SelectListItem { Text = x.Description, Value = x.Id.ToString(CultureInfo.InvariantCulture) });
            ViewBag.RolesList = lst;
            return PartialView("_Add");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddUser(AddUserModel userModel)
        {
            ModelState.Remove("BirthDay");
            if (!ModelState.IsValid)
                return PartialView("_ValidationSummery");

            var newUser = new User
            {
                Email = userModel.Email,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Password = Encryption.EncryptingPassword(userModel.Password),
                CreatedDate = DateAndTime.GetDateTime(),
                Role = _roleService.GetRoleByRoleId(userModel.RoleId),
                UserName = userModel.UserName,
                UserMetaData = new UserMetaData
                {
                    BirthDay = userModel.BirthDay,
                    Description = userModel.Description,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Major = userModel.Major,
                }
            };

            AddUserStatus status = _userService.Add(newUser);
            string message;
            if (status == AddUserStatus.AddingUserSuccessfully)
            {
                message = "کاربر جدید با موفقیت در سیستم ثبت شد";
                _uow.SaveChanges();
                return PartialView("_Alert",
                    new Alert { Message = message, Mode = AlertMode.Success });
            }
            switch (status)
            {
                case AddUserStatus.EmailExist:
                    message = "ایمیل وارد شده تکراری است";
                    break;
                case AddUserStatus.UserNameExist:
                    message = "نام کاربری تکراری است";
                    break;
                default:
                    message = "نام کاربری یا ایمیل تکراری است";
                    break;
            }
            return PartialView("_Alert", new Alert { Mode = AlertMode.Error, Message = message });
        }

        #endregion

        #region Edit User

        [HttpGet]
        public virtual ActionResult EditUser(int userId)
        {
            EditUserModel selectedUser = _userService.GetUserDataForEdit(userId);

            var model = new EditUserModel
            {
                BirthDay = selectedUser.BirthDay,
                Description = selectedUser.Description,
                Email = selectedUser.Email,
                FirstName = selectedUser.FirstName,
                LastName = selectedUser.LastName,
                Major = selectedUser.Major,
                UserName = selectedUser.UserName,
                Id = selectedUser.Id,
                RoleId = selectedUser.RoleId,
                IsBaned = selectedUser.IsBaned
            };

            model.AvatarStatus = (_avatarImage.Exist(selectedUser.UserName));

            ViewBag.Roles = new SelectList(_roleService.GetAllRoles(), "Id", "Description", model.RoleId);

            return PartialView("_EditUser", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditUser(EditUserModel userModel)
        {
            ModelState.Remove("BirthDay");
            if (!ModelState.IsValid)
                return PartialView("_ValidationSummery");

            var editedUser = new User
            {
                Email = userModel.Email,
                Id = userModel.Id,
                IsBaned = userModel.IsBaned,
                UserName = userModel.UserName,
                Role = _roleService.GetRoleByRoleId(userModel.RoleId),
                UserMetaData = new UserMetaData
                {
                    BirthDay = userModel.BirthDay,
                    Description = userModel.Description,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Major = userModel.Major
                }
            };

            if (!string.IsNullOrEmpty(userModel.Password))
            {
                editedUser.Password = userModel.Password;
                editedUser.LastPasswordChange = DateAndTime.GetDateTime();
            }
            if (userModel.IsBaned)
            {
                editedUser.BanedDate = DateAndTime.GetDateTime();
            }

            EditedUserStatus editingStatus = _userService.EditUser(editedUser);
            string message;
            switch (editingStatus)
            {
                case EditedUserStatus.UpdatingUserSuccessfully:
                    message = "اطلاعات کاربر با موفقیت به روز رسانی شد";
                    if (userModel.AvatarStatus == false)
                    {
                        _avatarImage.RemoveAvatarImage(userModel.UserName);
                    }
                    _uow.SaveChanges();
                    return PartialView("_Alert",
                        new Alert { Message = message, Mode = AlertMode.Success }); // user added successfully
                case EditedUserStatus.EmailExist:
                    message = "ایمیل وارد شده تکراری است";
                    break;
                case EditedUserStatus.UserNameExist:
                    message = "نام کاربری تکراری است";
                    break;
                default:
                    message = "نام کاربری یا ایمیل تکراری است";
                    break;
            }
            return PartialView("_Alert", new Alert { Mode = AlertMode.Error, Message = message });
        }

        #endregion
    }
}