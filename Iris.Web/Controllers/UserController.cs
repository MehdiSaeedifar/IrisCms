using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using CaptchaMvc.Attributes;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Utilities.Security;
using Iris.Web.Filters;
using Iris.Web.Infrastructure;
using Iris.Web.IrisMembership;

namespace Iris.Web.Controllers
{
    public partial class UserController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public UserController(IUnitOfWork uow, IUserService userService, IRoleService roleService, IFormsAuthenticationService formsAuthenticationService)
        {
            _uow = uow;
            _userService = userService;
            _roleService = roleService;
            _formsAuthenticationService = formsAuthenticationService;
        }

        public virtual ActionResult Index(string userName)
        {
            return View(_userService.Find(userName));
        }

        [SiteAuthorize]
        public virtual ActionResult ProfilePage()
        {
            return View();
        }

        [SiteAuthorize]
        public virtual ActionResult UserDetail()
        {
            return PartialView(MVC.User.Views._UserDetail, _userService.Find(User.Identity.Name));
        }

        [HttpGet]
        public virtual ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (Request.IsAjaxRequest())
                return PartialView(MVC.User.Views._LogOn);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return PartialView(MVC.User.Views._LogOn, model);
                return View(model);
            }

            string userName = string.Empty;
            int userId = 0;

            const string emailRegPattern =
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            string ip = Request.ServerVariables["REMOTE_ADDR"];
            VerifyUserStatus verificationResult = Regex.IsMatch(model.Identity, emailRegPattern)
                ? _userService.VerifyUserByEmail(model.Identity, model.Password, ref userName, ref userId, ip)
                : _userService.VerifyUserByUserName(model.Identity, model.Password, ref userName, ref userId, ip);

            switch (verificationResult)
            {
                case VerifyUserStatus.VerifiedSuccessfully:
                    {
                        string roleOfTheUser = _roleService.GetRoleByUserId(userId).Name;

                        // set user role cookie
                        SetAuthCookie(userName, roleOfTheUser, model.RememberMe);

                        _uow.SaveChanges();

                        if (Request.IsAjaxRequest())
                            return JavaScript(IsValidReturnUrl(returnUrl)
                                ? string.Format("window.location ='{0}';", returnUrl)
                                : "window.location.reload();");

                        return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
                    }
                case VerifyUserStatus.UserIsbaned:
                    ModelState.AddModelError("", "حساب کاربری شما مسدود است");
                    break;
                default:
                    ModelState.AddModelError("", "اطلاعات وارد شده صحیح نمی باشند");
                    break;
            }
            if (Request.IsAjaxRequest())
                return PartialView(MVC.User.Views._LogOn, model);
            return View(model);
        }

        [NonAction]
        private void SetAuthCookie(string userName, string roleofUser, bool presistantCookie)
        {
            //double timeout = presistantCookie ? FormsAuthentication.Timeout.TotalMinutes : 30;

            //DateTime now = DateTime.UtcNow.ToLocalTime();
            //TimeSpan expirationTimeSapne = TimeSpan.FromMinutes(timeout);

            //var authTicket = new FormsAuthenticationTicket(
            //    1,
            //    userName,
            //    now,
            //    now.Add(expirationTimeSapne),
            //    presistantCookie,
            //    roleofUser,
            //    FormsAuthentication.FormsCookiePath
            //    );

            //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            //{
            //    HttpOnly = true,
            //    Secure = FormsAuthentication.RequireSSL,
            //    Path = FormsAuthentication.FormsCookiePath
            //};

            //if (FormsAuthentication.CookieDomain != null)
            //{
            //    authCookie.Domain = FormsAuthentication.CookieDomain;
            //}

            //if (presistantCookie)
            //    authCookie.Expires = DateTime.Now.AddMinutes(timeout);

            //Response.Cookies.Add(authCookie);

            _formsAuthenticationService.SignIn(new User
            {
                UserName = userName,
                Role = new Role { Name = roleofUser }
            },
                presistantCookie);

        }

        [HttpGet]
        [SiteAuthorize]
        public virtual ActionResult UpdateProfile()
        {
            EditProfileModel selectedUser = _userService.GetProfileData(User.Identity.Name);

            AvatarImage.DefaultPath = Url.Content("~/Content/Images/user.gif");
            AvatarImage.BasePath = Url.Content("~/Content/avatars/");
            selectedUser.AvatarPath = AvatarImage.GetAvatarImage(User.Identity.Name);

            selectedUser.AvatarStatus = AvatarImage.Exist(User.Identity.Name);

            return PartialView(MVC.User.Views._EditProfile, selectedUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [SiteAuthorize]
        public virtual ActionResult UpdateProfile(EditProfileModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MVC.Shared.Views._ValidationSummery, model);

            if (!string.IsNullOrEmpty(model.OldPassword))
            {
                if (!_userService.Authenticate(User.Identity.Name, model.OldPassword))
                    return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
                    {
                        Message = "کلمه عبور اشتباه است",
                        Mode = AlertMode.Error
                    });
            }

            model.UserName = User.Identity.Name;
            EditedUserStatus updateResult = _userService.UpdateProfile(model);

            switch (updateResult)
            {
                case EditedUserStatus.UpdatingUserSuccessfully:
                    _uow.SaveChanges();
                    return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
                    {
                        Message = "اطلاعات حساب کاربری با موفقیت به روز رسانی شد",
                        Mode = AlertMode.Success
                    });
                case EditedUserStatus.EmailExist:
                    return PartialView(MVC.Admin.Shared.Views._Alert, new Alert
                    {
                        Message = "ایمیل تکراری است",
                        Mode = AlertMode.Error
                    });
            }

            return new EmptyResult();
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult RemoveAvatar()
        {
            AvatarImage.BasePath = Url.Content("~/Content/avatars/");
            AvatarImage.RemoveAvatarImage(User.Identity.Name);
            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = "آواتار مورد نظر با موفقیت حذف شد", Mode = AlertMode.Success });
        }

        [HttpPost]
        [Authorize]
        [AllowUploadSpecialFilesOnly(".jpg,.gif,.png")]
        public virtual ActionResult ChangeAvatar(HttpPostedFileBase avatarFile)
        {
            WebImage webImage = WebImage.GetImageFromRequest("avatarFile");

            var messages = new Dictionary<string, string>
            {
                {"invalid", "فایل نامعتبر می باشد"},
                {"success", "آواتار شما با موفقیت تعویض شد."}
            };

            if (!(webImage.Width > 0))
                return PartialView(MVC.Admin.Shared.Views._Alert,
                    new Alert { Message = messages["invalid"], Mode = AlertMode.Error });


            webImage = webImage.Resize(80, 85);

            webImage.FileName = User.Identity.Name + ".gif";

            string savingPath = Server.MapPath("~/Content/avatars/" + webImage.FileName);

            if (System.IO.File.Exists(savingPath))
                System.IO.File.Delete(savingPath);

            webImage.Save(savingPath, null, false);


            return PartialView(MVC.Admin.Shared.Views._Alert,
                new Alert { Message = messages["success"], Mode = AlertMode.Success });
        }

        [HttpGet]
        public virtual ActionResult Register()
        {
            if (Request.IsAjaxRequest())
                return PartialView(MVC.User.Views._Register);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaVerify("تصویر امنیتی وارد شده معتبر نیست")]
        public virtual ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(MVC.User.Views._Register, model);
            }
            var newUser = new User
            {
                CreatedDate = DateAndTime.GetDateTime(),
                Email = model.Email,
                IP = Request.ServerVariables["REMOTE_ADDR"],
                IsBaned = false,
                UserName = model.UserName,
                Password = Encryption.EncryptingPassword(model.Password),
                UserMetaData = new UserMetaData(),
                Role = _roleService.GetRoleByRoleName("user"),
                LastLoginDate = DateAndTime.GetDateTime()
            };

            AddUserStatus addingNewUserResult = _userService.Add(newUser);

            if (addingNewUserResult == AddUserStatus.EmailExist)
            {
                ModelState.AddModelError("", "ایمیل وارد شده قبلا درسیستم ثبت شده است.");
                return PartialView(MVC.User.Views._Register, model);
            }

            if (addingNewUserResult == AddUserStatus.UserNameExist)
            {
                ModelState.AddModelError("", "نام کاربری تکراری است.");
                return PartialView(MVC.User.Views._Register, model);
            }

            _uow.SaveChanges();

            SetAuthCookie(model.UserName, "user", false);

            return Json(new { result = "success" });
        }

        [HttpPost]
        public virtual ActionResult ExistsUserByUserName(string userName)
        {
            bool isExist = _userService.ExistsByUserName(userName);
            return Json(new { isExist });
        }

        [HttpPost]
        public virtual ActionResult ExistsUserByEmail(string email)
        {
            bool isExist = _userService.ExistsByEmail(email);
            return Json(new { isExist });
        }

        # region Validating ReturnUrl

        [NonAction]
        private bool IsValidReturnUrl(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                   && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\");
        }

        #endregion

        #region LogOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public virtual ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            if (Request.IsAjaxRequest())
                return Json(new { result = "true" });

            return RedirectToAction(MVC.User.ActionNames.LogOn, MVC.User.Name);
        }

        #endregion
    }
}