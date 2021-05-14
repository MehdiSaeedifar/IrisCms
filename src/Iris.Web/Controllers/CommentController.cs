﻿using System.Collections.Generic;
using System.Linq;
using DNTCaptcha.Core;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities;
using Iris.Utilities.DateAndTime;
using Iris.Web.Email;
using Iris.Web.Filters;
using Iris.Web.HtmlCleaner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Language = DNTCaptcha.Core.Language;

namespace Iris.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly IOptionService _optionService;
        private readonly IPageService _pageService;
        private readonly IPostService _postService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public CommentController(
            IUnitOfWork uow,
            ICommentService commentService,
            IOptionService optionService,
            IPostService postService,
            IUserService userService,
            IPageService pageService,
            IArticleService articleService,
            IEmailService emailService
        )
        {
            _uow = uow;
            _commentService = commentService;
            _optionService = optionService;
            _postService = postService;
            _pageService = pageService;
            _userService = userService;
            _articleService = articleService;
            _emailService = emailService;
        }

        #region Post Comment

        [HttpGet]
        public virtual ActionResult PostComments(int id)
        {
            IList<Comment> model;
            if (User.IsInRole("admin") || User.IsInRole("moderator") || User.IsInRole("editor"))
            {
                model = _commentService.GetCommentsForPost(id);
            }
            else
            {
                model = _commentService.GetCommentsForPost(id).Where(comment => comment.IsApproved).ToList();
            }
            ViewBag.PostId = id;
            return PartialView("_PostComments", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddUserPostComment(AddUserCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddUserPostComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value);
            }

            newComment.User = _userService.Find(User.Identity.Name);

            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            newComment.Post = _postService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();


            if (model.ReplyId != null)
            {

                string toUserName;
                string toEmail;

                if (newComment.Parent.User != null)
                {
                    toUserName = newComment.Parent.User.UserName;
                    toEmail = newComment.Parent.User.Email;
                }
                else
                {
                    toUserName = newComment.Parent.AnonymousUser.Name;
                    toEmail = newComment.Parent.AnonymousUser.Email;
                }

                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = newComment.User.UserName,
                    PostId = newComment.Post.Id,
                    PostTitle = newComment.Post.Title,
                    ToUserName = toUserName,
                    ToEmail = toEmail

                }, CommentReplyType.ReplyToPostComment);
            }


            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = User.Identity.Name,
                    date = DateAndTime.ConvertToPersian(DateAndTime.GetDateTime()),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "تصویر امنیتی وارد شده معتبر نیست.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public virtual ActionResult AddAnonymousPostComment(AddAnonymousCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddAnonymousPostComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value); ;
            }

            newComment.AnonymousUser = new AnonymousUser
            {
                Email = model.Email,
                Name = model.Name,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            newComment.Post = _postService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();

            if (model.ReplyId != null)
            {
                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = model.Name,
                    PostId = newComment.Post.Id,
                    PostTitle = newComment.Post.Title,
                    ToUserName = newComment.Parent.User?.UserName ?? newComment.Parent.AnonymousUser.Name,
                    ToEmail = newComment.Parent.User?.Email ?? newComment.Parent.AnonymousUser.Email

                }, CommentReplyType.ReplyToPostComment);
            }

            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = model.Name,
                    date = DateAndTime.ConvertToPersian(DateAndTime.GetDateTime()),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost("comment/addpostcomment/{id}")]
        public virtual ActionResult AddPostComment(int id, int? replyId)
        {
            if (!_postService.GetCommentStatus(id))
                return PartialView("_CommentClosed");
            ViewBag.ReplyId = replyId;
            ViewBag.PostId = id;
            return PartialView(User.Identity.IsAuthenticated
                ? "_AddUserPostComment"
                : "_AddAnonymousPostComment");
        }

        #endregion

        #region Page Comment

        [HttpGet]
        public virtual ActionResult PageComments(int id)
        {
            IList<Comment> model;
            if (User.IsInRole("admin") || User.IsInRole("moderator") || User.IsInRole("editor"))
            {
                model = _commentService.GetPageComments(id);
            }
            else
            {
                model = _commentService.GetPageComments(id, comment => comment.IsApproved).ToList();
            }
            ViewBag.PageId = id;
            return PartialView("_PageComments", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddUserPageComment(AddUserCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddUserPageComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0,
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value);
            }

            newComment.User = _userService.Find(User.Identity.Name);
            newComment.Page = _pageService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();


            if (model.ReplyId != null)
            {

                string toUserName;
                string toEmail;

                if (newComment.Parent.User != null)
                {
                    toUserName = newComment.Parent.User.UserName;
                    toEmail = newComment.Parent.User.Email;
                }
                else
                {
                    toUserName = newComment.Parent.AnonymousUser.Name;
                    toEmail = newComment.Parent.AnonymousUser.Email;
                }

                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = newComment.User.UserName,
                    PostId = newComment.Page.Id,
                    PostTitle = newComment.Page.Title,
                    ToUserName = toUserName,
                    ToEmail = toEmail

                }, CommentReplyType.ReplyToPageComment);
            }

            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = User.Identity.Name,
                    date = DateAndTime.ConvertToPersian(newComment.AddedDate),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "تصویر امنیتی وارد شده معتبر نیست.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public virtual ActionResult AddAnonymousPageComment(AddAnonymousCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddAnonymousPageComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value); ;
            }

            newComment.AnonymousUser = new AnonymousUser
            {
                Email = model.Email,
                Name = model.Name,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            newComment.Page = _pageService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();



            if (model.ReplyId != null)
            {
                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = model.Name,
                    PostId = newComment.Page.Id,
                    PostTitle = newComment.Page.Title,
                    ToUserName = newComment.Parent.User?.UserName ?? newComment.Parent.AnonymousUser.Name,
                    ToEmail = newComment.Parent.User?.Email ?? newComment.Parent.AnonymousUser.Email

                }, CommentReplyType.ReplyToPageComment);
            }


            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = model.Name,
                    date = DateAndTime.ConvertToPersian(newComment.AddedDate),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost("comment/addpagecomment/{id}")]
        public virtual ActionResult AddPageComment([FromRoute] int id, int? replyId)
        {
            if (!_pageService.GetCommentStatus(id))
                return PartialView("_CommentClosed");
            ViewBag.ReplyId = replyId;
            ViewBag.PageId = id;
            return PartialView(User.Identity.IsAuthenticated
                ? "_AddUserPageComment"
                : "_AddAnonymousPageComment");
        }

        #endregion

        #region Article Comment

        [HttpGet]
        public virtual ActionResult ArticleComments(int id)
        {
            IList<Comment> model;
            if (User.IsInRole("admin") || User.IsInRole("moderator") || User.IsInRole("editor"))
            {
                model = _commentService.GetArticleComments(id);
            }
            else
            {
                model = _commentService.GetArticleComments(id, comment => comment.IsApproved).ToList();
            }
            ViewBag.PageId = id;
            return PartialView("_ArticleComments", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddUserArticleComment(AddUserCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddUserArticleComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0,
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value); ;
            }

            newComment.User = _userService.Find(User.Identity.Name);
            newComment.Article = _articleService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();


            if (model.ReplyId != null)
            {

                string toUserName;
                string toEmail;

                if (newComment.Parent.User != null)
                {
                    toUserName = newComment.Parent.User.UserName;
                    toEmail = newComment.Parent.User.Email;
                }
                else
                {
                    toUserName = newComment.Parent.AnonymousUser.Name;
                    toEmail = newComment.Parent.AnonymousUser.Email;
                }

                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = newComment.User.UserName,
                    PostId = newComment.Article.Id,
                    PostTitle = newComment.Article.Title,
                    ToUserName = toUserName,
                    ToEmail = toEmail

                }, CommentReplyType.ReplyToArticleComment);
            }

            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = User.Identity.Name,
                    date = DateAndTime.ConvertToPersian(newComment.AddedDate),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "تصویر امنیتی وارد شده معتبر نیست.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public virtual ActionResult AddAnonymousArticleComment(AddAnonymousCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddAnonymousArticleComment", model);
            }
            var newComment = new Comment
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                LikeCount = 0
            };

            if (model.ReplyId != null)
            {
                newComment.Parent = _uow
                    .Set<Comment>()
                    .Include(comment => comment.User)
                    .Include(comment => comment.AnonymousUser)
                    .SingleOrDefault(comment => comment.Id == model.ReplyId.Value); ;
            }

            newComment.AnonymousUser = new AnonymousUser
            {
                Email = model.Email,
                Name = model.Name,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            newComment.Article = _articleService.Find(model.Id);
            _commentService.AddComment(newComment);
            _uow.SaveChanges();

            if (model.ReplyId != null)
            {
                _emailService.SendCommentReplyNotification(new CommentReplyEmailNotificationData
                {
                    CommentId = newComment.Id,
                    CommentText = newComment.Body,
                    FromUserName = model.Name,
                    PostId = newComment.Article.Id,
                    PostTitle = newComment.Article.Title,
                    ToUserName = newComment.Parent.User.UserName,
                    ToEmail = newComment.Parent.User.Email

                }, CommentReplyType.ReplyToArticleComment);
            }

            bool sendData = newComment.IsApproved = !_optionService.ModeratingComment;
            if (sendData)
            {
                return Json(new
                {
                    show = "true",
                    name = model.Name,
                    date = DateAndTime.ConvertToPersian(newComment.AddedDate),
                    likeCount = ConvertToPersian.ConvertToPersianString(0),
                    body = model.Body
                });
            }
            return Json(new { show = "false" });
        }

        [HttpPost("comment/addarticlecomment/{id}")]
        public virtual ActionResult AddArticleComment(int id, int? replyId)
        {
            if (!_articleService.GetCommentStatus(id))
                return PartialView("_CommentClosed");
            ViewBag.ReplyId = replyId;
            ViewBag.PageId = id;
            return PartialView(User.Identity.IsAuthenticated
                ? "_AddUserArticleComment"
                : "_AddAnonymousArticleComment");
        }

        #endregion

        public virtual ActionResult PostUserComments(int page = 0, int count = 10)
        {
            IList<PostCommentDetailModel> model = _commentService.GetUserPostComments(User.Identity.Name, page, count);
            ViewBag.Count = count;
            ViewBag.TotalRecords = _commentService.GetUserPostCommentsCount(User.Identity.Name);
            ViewBag.CurrentPage = page;
            return PartialView("_PostUserComments", model);
        }

        public virtual ActionResult PageUserComments(int page = 0, int count = 10)
        {
            IList<PageCommentDetailModel> model = _commentService.GetUserPageComments(User.Identity.Name, page, count);
            ViewBag.Count = count;
            ViewBag.TotalRecords = _commentService.GetUserPageCommentsCount(User.Identity.Name);
            ViewBag.CurrentPage = page;
            return PartialView("_PageUserComments", model);
        }

        public virtual ActionResult ArticleUserComments(int page = 0, int count = 10)
        {
            IList<ArticleCommentDetailModel> model = _commentService.GetUserArticleComments(User.Identity.Name, page,
                count);
            ViewBag.Count = count;
            ViewBag.TotalRecords = _commentService.GetUserArticleCommentsCount(User.Identity.Name);
            ViewBag.CurrentPage = page;
            return PartialView("_ArticleUserComments", model);
        }

        #region Rate Section

        [SiteAuthorize, HttpPost("comment/like/{id}")]
        public virtual ActionResult Like([FromRoute] int id)
        {
            string result;
            int likeCount = 0;
            if (!_commentService.IsUserLikeComment(id, User.Identity.Name))
            {
                likeCount = _commentService.Like(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }
            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount), commentId = id });
        }

        [SiteAuthorize, HttpPost("comment/dislike/{id}")]
        public virtual ActionResult DisLike([FromRoute] int id)
        {
            string result;
            int likeCount = 0;
            if (!_commentService.IsUserLikeComment(id, User.Identity.Name))
            {
                likeCount = _commentService.DisLike(id, _userService.Find(User.Identity.Name));
                _uow.SaveChanges();
                result = "success";
            }
            else
            {
                result = "duplicate";
            }
            return Json(new { result, count = ConvertToPersian.ConvertToPersianString(likeCount), commentId = id });
        }

        #endregion
    }
}