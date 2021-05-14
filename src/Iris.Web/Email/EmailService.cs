using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNTCommon.Web.Core;
using Iris.Model.EmailModel;
using Iris.Utilities.Mail;
using Iris.Web.Helpers;
using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using ICacheService = Iris.Web.Caching.ICacheService;

namespace Iris.Web.Email
{
    public class EmailService : IEmailService
    {
        private readonly ICacheService _cacheService;
        private readonly IViewConvertor _viewConvertor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebMailService _webMailService;

        public EmailService(
            ICacheService cacheService,
            IViewConvertor viewConvertor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IWebMailService webMailService
        )
        {
            _cacheService = cacheService;
            _viewConvertor = viewConvertor;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _webMailService = webMailService;
        }

        public SendingMailResult Send(MailDocument doc)
        {
            var config = _cacheService.GetSiteConfig();

            Task.Run(async () => await _webMailService.SendEmailAsync(new SmtpConfig
            {
                FromAddress = config.AdminEmail,
                FromName = config.AdminEmail,
                Username = config.MailServerLogin,
                Password = config.MailServerPass,
                Port = Convert.ToInt32(config.MailServerPort),
                Server = config.MailServerUrl,
                PickupFolder = "D://Emails",
                UsePickupFolder = false
            }, new List<MailAddress> { new() { ToAddress = doc.ToEmail } },
                doc.Subject, doc.Body, shouldValidateServerCertificate: false));


            return SendingMailResult.Successful;
        }


        public SendingMailResult SendCommentReplyNotification(CommentReplyEmailNotificationData data, CommentReplyType replyType)
        {
            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());

            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            var link = string.Empty;

            switch (replyType)
            {
                case CommentReplyType.ReplyToArticleComment:
                    link = urlHelper.Action("Index", "Article", new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, urlHelper.ActionContext.HttpContext.Request.Scheme);
                    break;
                case CommentReplyType.ReplyToPageComment:
                    link = urlHelper.Action("Index", "Page", new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, urlHelper.ActionContext.HttpContext.Request.Scheme);
                    break;
                case CommentReplyType.ReplyToPostComment:
                    link = urlHelper.Action("Index", "Post", new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, urlHelper.ActionContext.HttpContext.Request.Scheme);
                    break;
            }

            link += "#comment-" + data.CommentId;

            var model = new CommentReplyNotificationModel
            {
                FromUserName = data.FromUserName,
                ToUserName = data.ToUserName,
                PostId = data.PostId,
                PostTitle = data.PostTitle,
                CommentText = data.CommentText,
                CommentLink = link
            };

            var htmlText = _viewConvertor.RenderToStringAsync("EmailTemplates/_CommentReplyNotification", model).Result;


            var result = Send(new MailDocument
            {
                Body = htmlText,
                Subject =
                     string.Format("شما پاسخی از طرف {0} در مطلب {1} دریافت کرده اید.", data.FromUserName, data.PostTitle),
                ToEmail = data.ToEmail
            });


            return result;
        }

        public SendingMailResult SendResetPasswordConfirmationEmail(string userName, string email, string key)
        {
            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());

            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);


            var resetLink = urlHelper.Action("ResetPassword", "ForgottenPassword", new
            {
                key
            }, urlHelper.ActionContext.HttpContext.Request.Scheme);


            var model = new ResetPasswordModel
            {
                UserName = userName,
                ResetLink = resetLink
            };



            var htmlText = _viewConvertor.RenderToStringAsync("EmailTemplates/_ResetPasswordConfirmation", model).Result;


            var result = Send(new MailDocument
            {
                Body = htmlText,
                Subject =
                   "تایید کردن بازنشانی کلمه عبور",
                ToEmail = email
            });


            return result;
        }

        public SendingMailResult SendNewPassword(string userName, string email, string newPassword)
        {

            var model = new NewPasswordModel
            {
                UserName = userName,
                NewPassword = newPassword,
            };

            var htmlText = _viewConvertor.RenderToStringAsync("EmailTemplates/_NewPassword", model).Result;


            var result = Send(new MailDocument
            {
                Body = htmlText,
                Subject = "کلمه عبور جدید",
                ToEmail = email
            });


            return result;
        }
    }
}
