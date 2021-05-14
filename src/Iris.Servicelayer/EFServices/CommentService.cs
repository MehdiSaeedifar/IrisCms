using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EFCoreSecondLevelCacheInterceptor;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Model.RSSModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iris.Servicelayer.EFServices
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Comment> _comments;

        public CommentService(IUnitOfWork uow)
        {
            _uow = uow;
            _comments = uow.Set<Comment>();
        }

        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }

        public void Remove(int id)
        {
            var selectedComment = _comments.Find(id);
            _comments.Where(x => x.ParentId == id).Load();
            _comments.Remove(selectedComment);
        }

        public IList<Comment> GetAll(Func<Comment, bool> expression)
        {
            return _comments.Where(expression).ToList();
        }

        public IList<Comment> GetAllComments()
        {
            return _comments.ToList();
        }

        public Comment GetComment(int id)
        {
            return _comments.Find(id);
        }

        public IList<Comment> GetAllCommentsByPostId(long id)
        {
            return _comments.Where(x => x.Post.Id == id).ToList();
        }

        public void Approve(int id)
        {
            _comments.Find(id).IsApproved = true;
        }

        public void DisApprove(int id)
        {
            _comments.Find(id).IsApproved = false;
        }

        public int DisLike(int id, User user)
        {
            var selectedComment = _comments.Find(id);

            selectedComment.LikeCount -= 1;

            _uow.Set<LikeUsersComment>().Add(new LikeUsersComment
            {
                User = user,
                Comment = selectedComment
            });

            return selectedComment.LikeCount;
        }

        public int Like(int id, User user)
        {
            var selectedComment = _comments.Find(id);

            selectedComment.LikeCount += 1;

            _uow.Set<LikeUsersComment>().Add(new LikeUsersComment
            {
                User = user,
                Comment = selectedComment
            });

            return selectedComment.LikeCount;
        }


        public IList<CommentDataTableModel> GetDataTable(string term, int page, int count, Order order
            , CommentOrderBy orderBy, CommentSearchBy searchBy)
        {
            IQueryable<Comment> selectedComments =
                _comments.Include(comment => comment.Post)
                    .Include(comment => comment.User).AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case CommentSearchBy.UserName:
                        selectedComments =
                            selectedComments.Where(comment => comment.User.UserName.Contains(term)).AsQueryable();
                        break;
                    case CommentSearchBy.Author:
                        selectedComments = selectedComments.Where(comment => comment.AnonymousUser
                            .Name.Contains(term)).AsQueryable();
                        break;
                    case CommentSearchBy.Body:
                        selectedComments = selectedComments.Where(comment => comment.Body.Contains(term)).AsQueryable();
                        break;
                    case CommentSearchBy.Ip:
                        selectedComments = selectedComments.Where(comment => comment.User.IP.Contains(term)).
                            Where(comment => comment.AnonymousUser.IP.Contains(term))
                            .AsQueryable();
                        break;
                }
            }

            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case CommentOrderBy.AddDate:
                        selectedComments = selectedComments.OrderBy(comment => comment.AddedDate).AsQueryable();
                        break;
                    case CommentOrderBy.IsApproved:
                        selectedComments = selectedComments.OrderBy(comment => comment.IsApproved).AsQueryable();
                        break;
                    case CommentOrderBy.LikeCount:
                        selectedComments = selectedComments.OrderBy(comment => comment.LikeCount).AsQueryable();
                        break;
                    case CommentOrderBy.UserName:
                        selectedComments = selectedComments.OrderBy(comment => comment.User.UserName).AsQueryable();
                        break;
                    case CommentOrderBy.Author:
                        selectedComments = selectedComments.OrderBy(comment => comment.AnonymousUser.Name).AsQueryable();
                        break;
                    case CommentOrderBy.Ip:
                        selectedComments = selectedComments.OrderBy(comment => comment.User.IP)
                            .ThenBy(comment => comment.AnonymousUser.IP).AsQueryable();
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case CommentOrderBy.AddDate:
                        selectedComments =
                            selectedComments.OrderByDescending(comment => comment.AddedDate).AsQueryable();
                        break;
                    case CommentOrderBy.IsApproved:
                        selectedComments =
                            selectedComments.OrderByDescending(comment => comment.IsApproved).AsQueryable();
                        break;
                    case CommentOrderBy.LikeCount:
                        selectedComments =
                            selectedComments.OrderByDescending(comment => comment.LikeCount).AsQueryable();
                        break;
                    case CommentOrderBy.UserName:
                        selectedComments =
                            selectedComments.OrderByDescending(comment => comment.User.UserName).AsQueryable();
                        break;
                    case CommentOrderBy.Author:
                        selectedComments =
                            selectedComments.OrderByDescending(comment => comment.AnonymousUser.Name).AsQueryable();
                        break;
                    case CommentOrderBy.Ip:
                        selectedComments = selectedComments.OrderByDescending(comment => comment.User.IP)
                            .ThenByDescending(comment => comment.AnonymousUser.IP)
                            .AsQueryable();
                        break;
                }
            }

            return selectedComments.Skip(page * count).Take(count).Select(comment => new CommentDataTableModel
            {
                AddDate = comment.AddedDate,
                AuthorName = comment.AnonymousUser.Name,
                Body = comment.Body,
                Id = comment.Id,
                PostTitle = comment.Post.Title,
                AuthorId = comment.AnonymousUser.Id,
                UserName = comment.User.UserName,
                IsApproved = comment.IsApproved,
                LikeCount = comment.LikeCount,
                PostId = comment.Post.Id,
                UserId = comment.User.Id
            }).ToList();
        }


        public int Count
        {
            get { return _comments.Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1)).Count(); }
        }


        public void Update(Comment comment)
        {
            Comment selectedComment = _comments.Find(comment.Id);
            selectedComment.Body = comment.Body;
        }

        public IList<Comment> GetCommentsForPost(int postId)
        {
            return _comments.Where(comment => comment.Post.Id == postId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .OrderBy(comment => comment.AddedDate)
                .ToList().Where(comment => comment.Parent == null).ToList();
        }


        public Comment Find(int id)
        {
            return _comments.Find(id);
        }

        public IList<Comment> GetCommentsForPost(int postId, Expression<Func<Comment, bool>> expression)
        {
            return _comments.Where(comment => comment.Post.Id == postId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .OrderBy(comment => comment.AddedDate)
                .Where(comment => comment.IsApproved)
                .Where(comment => comment.Parent == null).ToList();
        }


        public bool IsUserLikeComment(int id, string userName)
        {
            return _comments.Where(comment => comment.Id == id).Any(comment =>
                comment.LikedUsers.Any(likedUser => likedUser.User.UserName == userName)
            );
        }


        public int GetCommentsCount(int postId)
        {
            return _comments.Count(comment => comment.Post.Id == postId);
        }

        public IList<Comment> GetPageComments(int pageId)
        {
            return _comments.Where(comment => comment.Page.Id == pageId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .OrderBy(comment => comment.AddedDate)
                .ToList().Where(comment => comment.Parent == null).ToList();
        }

        public IList<Comment> GetPageComments(int pageId, Expression<Func<Comment, bool>> expression)
        {
            return _comments.Where(comment => comment.Page.Id == pageId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .Where(expression)
                .OrderBy(comment => comment.AddedDate)
                .ToList().Where(comment => comment.Parent == null).ToList();
        }

        public IList<Comment> GetArticleComments(int articleId)
        {
            return _comments.Where(comment => comment.Article.Id == articleId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .OrderBy(comment => comment.AddedDate)
                .ToList().Where(comment => comment.Parent == null).ToList();
        }

        public IList<Comment> GetArticleComments(int articleId, Expression<Func<Comment, bool>> expression)
        {
            return _comments.Where(comment => comment.Article.Id == articleId)
                .Include(comment => comment.User)
                .Include(comment => comment.AnonymousUser)
                .Include(comment => comment.Children)
                .Include(comment => comment.User.Role)
                .Where(expression)
                .OrderBy(comment => comment.AddedDate)
                .ToList().Where(comment => comment.Parent == null).ToList();
        }

        public IList<RssCommentModel> GetRssComments(int count)
        {
            return _comments.AsNoTracking().Where(comment => comment.Post != null).Select(comment => new RssCommentModel
            {
                Body = comment.Body,
                CreatedDate = comment.AddedDate,
                Id = comment.Post.Id,
                Title = comment.Post.Title,
                Author = comment.User.UserName ?? comment.AnonymousUser.Name
            }).OrderByDescending(comment => comment.CreatedDate).Take(count)
                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1)).ToList();
        }

        public IList<PostCommentDetailModel> GetUserPostComments(string userName, int page, int count)
        {
            return _comments.AsNoTracking().Where(comment => comment.User.UserName == userName && comment.Post != null)
                .Select(comment => new PostCommentDetailModel
                {
                    AddedDate = comment.AddedDate,
                    Body = comment.Body,
                    Id = comment.Id,
                    LikeCount = comment.LikeCount,
                    PostId = comment.Post.Id,
                    Title = comment.Post.Title
                }).OrderByDescending(comment => comment.AddedDate).Skip(page * count).Take(count).ToList();
        }


        public int GetUserPostCommentsCount(string userName)
        {
            return _comments.Count(comment => comment.User.UserName == userName && comment.Post != null);
        }

        public IList<PageCommentDetailModel> GetUserPageComments(string userName, int page, int count)
        {
            return _comments.AsNoTracking().Where(comment => comment.User.UserName == userName && comment.Page != null)
                .Select(comment => new PageCommentDetailModel
                {
                    AddedDate = comment.AddedDate,
                    Body = comment.Body,
                    Id = comment.Id,
                    LikeCount = comment.LikeCount,
                    PageId = comment.Page.Id,
                    Title = comment.Page.Title
                }).OrderByDescending(comment => comment.AddedDate).Skip(page * count).Take(count).ToList();
        }

        public int GetUserPageCommentsCount(string userName)
        {
            return _comments.Count(comment => comment.User.UserName == userName && comment.Page != null);
        }

        public IList<ArticleCommentDetailModel> GetUserArticleComments(string userName, int page, int count)
        {
            return
                _comments.AsNoTracking().Where(comment => comment.User.UserName == userName && comment.Article != null)
                    .Select(comment => new ArticleCommentDetailModel
                    {
                        AddedDate = comment.AddedDate,
                        Body = comment.Body,
                        Id = comment.Id,
                        LikeCount = comment.LikeCount,
                        ArticleId = comment.Article.Id,
                        Title = comment.Article.Title
                    }).OrderByDescending(comment => comment.AddedDate).Skip(page * count).Take(count).ToList();
        }

        public int GetUserArticleCommentsCount(string userName)
        {
            return _comments.Count(comment => comment.User.UserName == userName && comment.Article != null);
        }

        public void LikeComment(long id)
        {
            _comments.Find(id).LikeCount += 1;
        }
    }
}