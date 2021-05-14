using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCore.Unobtrusive.Ajax;
using DNTCaptcha.Core;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Iris.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAvatarImage _avatarImage;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public UserController(
            IUnitOfWork uow,
            IUserService userService,
            IRoleService roleService,
            IAvatarImage avatarImage,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _uow = uow;
            _userService = userService;
            _roleService = roleService;
            _avatarImage = avatarImage;
            _webHostEnvironment = webHostEnvironment;
        }

        public virtual ActionResult Index(string userName)
        {
            return View(_uow.Set<User>().AsSplitQuery().Include(user => user.Role)
                .Include(user => user.UserMetaData)
                .Include(user => user.Posts)
                .Include(user => user.Pages)
                .Include(user => user.Articles)
                .Include(user => user.Comments)
                .FirstOrDefault(user => user.UserName.Equals(userName)));
        }

        [SiteAuthorize]
        public virtual ActionResult ProfilePage()
        {
            return View();
        }

        [SiteAuthorize]
        public virtual ActionResult UserDetail()
        {
            return PartialView("_UserDetail", _uow.Set<User>().Include(user => user.Role)
                .Include(user => user.UserMetaData)
                .FirstOrDefault(user => user.UserName.Equals(User.Identity.Name)));
        }

        [HttpGet]
        public virtual ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (Request.IsAjaxRequest())
                return PartialView("_LogOn");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> LogOn(LogOnModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return PartialView("_LogOn", model);
                return View(model);
            }

            string userName = string.Empty;
            int userId = 0;

            const string emailRegPattern =
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            VerifyUserStatus verificationResult = Regex.IsMatch(model.Identity, emailRegPattern)
                ? _userService.VerifyUserByEmail(model.Identity, model.Password, ref userName, ref userId, ip)
                : _userService.VerifyUserByUserName(model.Identity, model.Password, ref userName, ref userId, ip);

            switch (verificationResult)
            {
                case VerifyUserStatus.VerifiedSuccessfully:
                    {
                        string roleOfTheUser = _roleService.GetRoleByUserId(userId).Name;

                        // set user role cookie
                        await SetAuthCookie(userName, roleOfTheUser, model.RememberMe);

                        _uow.SaveChanges();

                        if (Request.IsAjaxRequest())
                            return Content(IsValidReturnUrl(returnUrl)
                                ? string.Format("<script language='javascript' type='text/javascript'>window.location ='{0}';</script>", returnUrl)
                                : "<script language='javascript' type='text/javascript'>window.location.reload();</script>");

                        return RedirectToAction("Index", "Home");
                    }
                case VerifyUserStatus.UserIsbaned:
                    ModelState.AddModelError("", "حساب کاربری شما مسدود است");
                    break;
                default:
                    ModelState.AddModelError("", "اطلاعات وارد شده صحیح نمی باشند");
                    break;
            }
            if (Request.IsAjaxRequest())
                return PartialView("_LogOn", model);
            return View(model);
        }

        [NonAction]
        private async Task SetAuthCookie(string userName, string roleofUser, bool presistantCookie)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, roleofUser),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

                //RedirectUri = "/Home/Index",
                ExpiresUtc = DateTimeOffset.MaxValue,
                IsPersistent = presistantCookie,
                IssuedUtc = DateTimeOffset.Now
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


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

            //_formsAuthenticationService.SignIn(new User
            //{
            //    UserName = userName,
            //    Role = new Role { Name = roleofUser }
            //},
            //    presistantCookie);

        }

        [HttpGet]
        [SiteAuthorize]
        public virtual ActionResult UpdateProfile()
        {
            EditProfileModel selectedUser = _userService.GetProfileData(User.Identity.Name);

            selectedUser.AvatarPath = _avatarImage.GetAvatarImage(User.Identity.Name);

            selectedUser.AvatarStatus = _avatarImage.Exist(User.Identity.Name);

            return PartialView("_EditProfile", selectedUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [SiteAuthorize]
        public virtual ActionResult UpdateProfile(EditProfileModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_ValidationSummery", model);

            if (!string.IsNullOrEmpty(model.OldPassword))
            {
                if (!_userService.Authenticate(User.Identity.Name, model.OldPassword))
                    return PartialView("_Alert", new Alert
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
                    return PartialView("_Alert", new Alert
                    {
                        Message = "اطلاعات حساب کاربری با موفقیت به روز رسانی شد",
                        Mode = AlertMode.Success
                    });
                case EditedUserStatus.EmailExist:
                    return PartialView("_Alert", new Alert
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
            _avatarImage.RemoveAvatarImage(User.Identity.Name);
            return PartialView("_Alert",
                new Alert { Message = "آواتار مورد نظر با موفقیت حذف شد", Mode = AlertMode.Success });
        }

        [HttpPost]
        [Authorize]
        [AllowUploadSpecialFilesOnly(".jpg,.gif,.png")]
        public virtual ActionResult ChangeAvatar(IFormFile avatarFile)
        {
            var messages = new Dictionary<string, string>
            {
                {"invalid", "فایل نامعتبر می باشد"},
                {"success", "آواتار شما با موفقیت تعویض شد."}
            };

            using var image = Image.Load(avatarFile.OpenReadStream());

            if (!(image.Width > 0))
                return PartialView("_Alert",
                    new Alert { Message = messages["invalid"], Mode = AlertMode.Error });


            image.Mutate(x => x.Resize(100, 100));


            string savingPath = Path.Combine(_webHostEnvironment.WebRootPath, "Content/avatars/",
                User.Identity.Name + ".gif");

            if (System.IO.File.Exists(savingPath))
                System.IO.File.Delete(savingPath);

            image.SaveAsGif(savingPath);


            return PartialView("_Alert",
                new Alert { Message = messages["success"], Mode = AlertMode.Success });
        }

        [HttpGet]
        public virtual ActionResult Register()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_Register");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "تصویر امنیتی وارد شده معتبر نیست.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Register", model);
            }
            var newUser = new User
            {
                CreatedDate = DateAndTime.GetDateTime(),
                Email = model.Email,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
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
                return PartialView("_Register", model);
            }

            if (addingNewUserResult == AddUserStatus.UserNameExist)
            {
                ModelState.AddModelError("", "نام کاربری تکراری است.");
                return PartialView("_Register", model);
            }

            _uow.SaveChanges();

            await SetAuthCookie(model.UserName, "user", true);

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
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            if (Request.IsAjaxRequest())
                return Json(new { result = "true" });

            return RedirectToAction("LogOn", "User");
        }

        #endregion
    }
}