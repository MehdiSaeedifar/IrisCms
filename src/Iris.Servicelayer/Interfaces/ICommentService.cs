using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Model.RSSModel;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Servicelayer.Interfaces
{
    public interface ICommentService
    {
        int Count { get; }
        void AddComment(Comment comment);
        void Remove(int id);
        void Update(Comment comment);
        IList<Comment> GetAll(Func<Comment, bool> expression);
        IList<Comment> GetAllComments();
        Comment GetComment(int id);
        IList<Comment> GetAllCommentsByPostId(long id);
        void Approve(int id);
        void DisApprove(int id);
        int Like(int id, User user);
        int DisLike(int id, User user);

        IList<CommentDataTableModel> GetDataTable(string term, int page, int count, Order order, CommentOrderBy orderBy,
            CommentSearchBy searchBy);

        IList<Comment> GetCommentsForPost(int postId);
        IList<Comment> GetCommentsForPost(int postId, Expression<Func<Comment, bool>> expression);
        Comment Find(int id);
        bool IsUserLikeComment(int id, string userName);
        int GetCommentsCount(int postId);
        IList<Comment> GetPageComments(int pageId);
        IList<Comment> GetPageComments(int pageId, Expression<Func<Comment, bool>> expression);
        IList<Comment> GetArticleComments(int articleId);
        IList<Comment> GetArticleComments(int articleId, Expression<Func<Comment, bool>> expression);
        IList<RssCommentModel> GetRssComments(int count);
        IList<PostCommentDetailModel> GetUserPostComments(string userName, int page, int count);
        int GetUserPostCommentsCount(string userName);
        IList<PageCommentDetailModel> GetUserPageComments(string userName, int page, int count);
        int GetUserPageCommentsCount(string userName);
        IList<ArticleCommentDetailModel> GetUserArticleComments(string userName, int page, int count);
        int GetUserArticleCommentsCount(string userName);
    }
}