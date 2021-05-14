using System;
using System.Globalization;
using DNTCaptcha.Core;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Utilities.Mail;
using Iris.Utilities.Security;
using Iris.Web.Email;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public partial class ForgottenPasswordController : Controller
    {
        private readonly IForgottenPasswordService _forgttenPasswordService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public ForgottenPasswordController(IUnitOfWork uow, IUserService userService,
            IForgottenPasswordService forgottenPasswordService, IEmailService emailService)
        {
            _uow = uow;
            _userService = userService;
            _forgttenPasswordService = forgottenPasswordService;
            _emailService = emailService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return PartialView("_Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "تصویر امنیتی وارد شده معتبر نیست.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public virtual ActionResult Index(ForgottenPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Index", model);
            }

            bool isEmailExist = _userService.ExistsByEmail(model.Email);

            if (isEmailExist)
            {
                User selecteduser = _userService.GetUserByEmail(model.Email);
                string key = Guid.NewGuid().ToString();
                var newRequestTicket = new ForgottenPassword
                {
                    User = selecteduser,
                    Key = key,
                    ResetDateTime = DateAndTime.GetDateTime()
                };

                _forgttenPasswordService.Add(newRequestTicket);

                if (_emailService.SendResetPasswordConfirmationEmail(selecteduser.UserName, model.Email, key)
                     == SendingMailResult.Successful)
                {
                    _uow.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        result = "true",
                        message = "متاسفانه خطایی در ارسال ایمیل رخ داده است."
                    });
                }

                return Json(new
                {
                    result = "true",
                    message = "ایمیلی برای تایید بازنشانی کلمه عبور برای شما ارسال شد.اعتبارایمیل ارسالی 24 ساعت است."
                });
            }

            return Json(new
            {
                result = "false",
                message = "این ایمیل در سیستم ثبت نشده است"
            });
        }

        public virtual ActionResult ResetPassword(string key)
        {
            if (_forgttenPasswordService.RequestDate(key).Subtract(DateAndTime.GetDateTime()).TotalDays > 1)
            {
                return View();
            }
            string newPass = new Random().Next(500000, 10000000).ToString(CultureInfo.InvariantCulture);
            User selectedUser = _forgttenPasswordService.FindUser(key);
            _forgttenPasswordService.Remove(key);
            selectedUser.Password = Encryption.EncryptingPassword(newPass);
            selectedUser.LastPasswordChange = DateAndTime.GetDateTime();

            if (_emailService.SendNewPassword(selectedUser.UserName, selectedUser.Email, newPass) ==
                SendingMailResult.Successful)
            {
                _uow.SaveChanges();
            }
            else
            {
                ViewBag.Message = "متاسفانه خطایی در ارسال ایمیل رخ داده است.";
                return View();
            }

            ViewBag.Message = "کلمه عبور شما با موفقیت باز نشانی شد و به ایمیل شما ارسال گردید";

            return View();
        }
    }
}