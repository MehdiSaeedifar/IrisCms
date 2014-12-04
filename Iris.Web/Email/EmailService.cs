using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Razor;
using System.Web.Routing;
using Iris.DomainClasses.Entities;
using Iris.Model.EmailModel;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Mail;
using Iris.Web.Caching;
using Iris.Web.Helpers;
using Iris.Web.Infrastructure;

namespace Iris.Web.Email
{
    public class EmailService : IEmailService
    {
        private readonly ICacheService _cacheService;
        private readonly IViewConvertor _viewConvertor;
        private readonly HttpContextBase _httpContext;
        public EmailService(ICacheService cacheService, IViewConvertor viewConvertor, HttpContextBase httpContext)
        {
            _cacheService = cacheService;
            _viewConvertor = viewConvertor;
            _httpContext = httpContext;
        }

        public SendingMailResult Send(MailDocument doc)
        {
            var config = _cacheService.GetSiteConfig();

            EMail.EnableSsl = false;
            EMail.SmtpPort = Convert.ToInt32(config.MailServerPort);
            EMail.From = config.AdminEmail;
            EMail.Password = config.MailServerPass;
            EMail.UserName = config.MailServerLogin;
            EMail.SmtpServer = config.MailServerUrl;

            string error = string.Empty;




            return EMail.Send(doc.ToEmail, doc.Subject, doc.Body, ref error);
        }


        public SendingMailResult SendCommentReplyNotification(CommentReplyEmailNotificationData data, CommentReplyType replyType)
        {
            var urlHelper = new UrlHelper(_httpContext.Request.RequestContext);

            var link = string.Empty;

            switch (replyType)
            {
                case CommentReplyType.ReplyToArticleComment:
                    link = urlHelper.Action(MVC.Article.ActionNames.Index, MVC.Article.Name, new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, _httpContext.Request.Url.Scheme);
                    break;
                case CommentReplyType.ReplyToPageComment:
                    link = urlHelper.Action(MVC.Page.ActionNames.Index, MVC.Page.Name, new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, _httpContext.Request.Url.Scheme);
                    break;
                case CommentReplyType.ReplyToPostComment:
                    link = urlHelper.Action(MVC.Post.ActionNames.Index, MVC.Post.Name, new
                    {
                        title = UrlExtensions.ResolveTitleForUrl(data.PostTitle),
                        id = data.PostId
                    }, _httpContext.Request.Url.Scheme);
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

            var htmlText = _viewConvertor.RenderRazorViewToString(MVC.EmailTemplates.Views._CommentReplyNotification, model);


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
            var urlHelper = new UrlHelper(_httpContext.Request.RequestContext);


            var resetLink = urlHelper.Action(MVC.ForgottenPassword.ActionNames.ResetPassword, MVC.ForgottenPassword.Name, new
                    {
                        key
                    }, _httpContext.Request.Url.Scheme);


            var model = new ResetPasswordModel
            {
                UserName = userName,
                ResetLink = resetLink
            };



            var htmlText = _viewConvertor.RenderRazorViewToString(MVC.EmailTemplates.Views._ResetPasswordConfirmation, model);


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

            var htmlText = _viewConvertor.RenderRazorViewToString(MVC.EmailTemplates.Views._NewPassword, model);


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