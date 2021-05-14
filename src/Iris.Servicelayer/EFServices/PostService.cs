using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EFCoreSecondLevelCacheInterceptor;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Model.RSSModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Post> _posts;

        public PostService(IUnitOfWork uow)
        {
            _uow = uow;
            _posts = uow.Set<Post>();
        }

        public void AddPost(Post post)
        {
            post.Like = 0;
            post.VisitedNumber = 0;
            _posts.Add(post);
        }

        public void RemovePostById(int id)
        {
            _posts.Remove(_posts.Find(id));
        }

        public EditPostModel GetPostForEdit(int id)
        {
            EditPostModel postModel =
                _posts.Where(post => post.Id == id)
                    .Select(
                        post =>
                            new EditPostModel
                            {
                                Book = post.Book,
                                BookImage = post.Book.Image,
                                Labels = post.Labels.Select(labelPost => labelPost.Label).ToList(),
                                PostBody = post.Body,
                                PostCommentStatus = post.CommentStatus,
                                PostDescription = post.Description,
                                PostId = post.Id,
                                PostKeyword = post.Keyword,
                                PostTitle = post.Title,
                                DownloadLinks = post.DownloadLinks,
                                PostStatus = post.Status
                            })
                    .FirstOrDefault();
            return postModel;
        }


        public UpdatePostStatus UpdatePost(EditPostModel postModel)
        {
            Post selectedPost = _uow.Set<Post>()
                .Include(post => post.Labels)
                .Include(post => post.Book)
                .Include(post => post.Book.Image)
                .Include(post => post.DownloadLinks)
                .SingleOrDefault(post => post.Id == postModel.PostId);

            int count = selectedPost.Labels.Count;
            for (int i = 0; i < count; i++)
            {
                selectedPost.Labels.Remove(selectedPost.Labels.ElementAt(i));
                count--;
            }

            selectedPost.Labels = postModel.Labels.Select(label => new LabelPost
            {
                Label = label
            }).ToList();

            selectedPost.DownloadLinks = postModel.DownloadLinks;

            selectedPost.Body = postModel.PostBody;
            selectedPost.Book.Author = postModel.Book.Author;
            selectedPost.Book.ISBN = postModel.Book.ISBN;
            selectedPost.Book.Language = postModel.Book.Language;
            selectedPost.Book.Name = postModel.Book.Name;
            selectedPost.Book.Page = postModel.Book.Page;
            selectedPost.Book.Year = postModel.Book.Year;
            selectedPost.Book.Description = postModel.Book.Description;
            selectedPost.Book.Publisher = postModel.Book.Publisher;

            selectedPost.Book.Image.Description = postModel.BookImage.Description;
            selectedPost.Book.Image.Path = postModel.BookImage.Path;
            selectedPost.Book.Image.Title = postModel.BookImage.Title;

            selectedPost.CommentStatus = postModel.PostCommentStatus;
            selectedPost.Description = postModel.PostDescription;
            selectedPost.Keyword = postModel.PostKeyword;
            selectedPost.ModifiedDate = postModel.ModifiedDate;
            selectedPost.Status = postModel.PostStatus;
            selectedPost.Title = postModel.PostTitle;

            return UpdatePostStatus.Successfull;
        }

        public IList<BooksListModel> GetBooksList(int page, int count)
        {
            if (count > 20)
            {
                count = 20;
            }

            var resultsToSkip = page * count;

            var posts = _posts.AsNoTracking()
                 .Where(x => x.Status == "visible")
                 .OrderByDescending(post => post.CreatedDate)
                 .Skip(resultsToSkip)
                 .Take(count)
                 .Select(post => new BooksListModel
                 {
                     Body = post.Body,
                     BookName = post.Book.Name,
                     CommentsCount = post.Comments.Count,
                     CreatedDate = post.CreatedDate,
                     PostId = post.Id,
                     ImageDescription = post.Book.Image.Description,
                     ImagePath = post.Book.Image.Path,
                     ImageTitle = post.Book.Image.Title,
                     LikeCount = post.Like,
                     UserName = post.User.UserName,
                     UserId = post.User.Id,
                     VisitedCount = post.VisitedNumber,
                     Title = post.Title
                 })
                 .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                 .ToList();

            var postIds = posts.Select(p => p.PostId).ToArray();

            var labelPosts = _uow.Set<LabelPost>()
                .AsNoTracking()
                .Where(labelPost => postIds.Contains(labelPost.PostId))
                .Include(labelPost => labelPost.Label)
                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                .ToList()
                .GroupBy(labelPost => labelPost.PostId)
                .ToDictionary(g => g.Key, g => g.Select(labelPost => labelPost.Label).ToList());

            foreach (var post in posts)
            {
                post.Lables = labelPosts.GetValueOrDefault(post.PostId, new List<Label>());
            }

            return posts;
        }

        public IList<BooksListModel> GetBooksList(int labelId, int page, int count)
        {
            if (count > 20)
            {
                count = 20;
            }

            var resultsToSkip = page * count;

            return _posts.AsNoTracking()
                .Where(post => post.Labels.Any(labelPost => labelPost.LabelId == labelId) && post.Status == "visible")
                .OrderByDescending(post => post.CreatedDate)
                .Select(post => new BooksListModel
                {
                    Body = post.Body,
                    BookName = post.Book.Name,
                    CommentsCount = post.Comments.Count,
                    CreatedDate = post.CreatedDate,
                    PostId = post.Id,
                    ImageDescription = post.Book.Image.Description,
                    ImagePath = post.Book.Image.Path,
                    ImageTitle = post.Book.Image.Title,
                    LikeCount = post.Like,
                    Lables = post.Labels.Select(labelPost => labelPost.Label).ToList(),
                    UserName = post.User.UserName,
                    UserId = post.User.Id,
                    VisitedCount = post.VisitedNumber,
                    Title = post.Title
                })
                .Skip(resultsToSkip)
                .Take(count)
                .ToList();
        }

        public PostModel GetPost(int id)
        {
            return _posts.AsNoTracking().Where(post => post.Id == id).Select(post => new PostModel
            {
                BookBody = post.Book.Description,
                BookName = post.Book.Name,
                BookAuthor = post.Book.Author,
                BookDate = post.Book.Year,
                BookIsbn = post.Book.ISBN,
                BookLanguage = post.Book.Language,
                BookPageCount = post.Book.Page,
                BookPublisher = post.Book.Publisher,
                CommentsCount = post.Comments.Count,
                CommentStatus = post.CommentStatus,
                CreatedDate = post.CreatedDate,
                Id = post.Id,
                DownloadLinks = post.DownloadLinks,
                ImagePath = post.Book.Image.Path,
                ImageDescription = post.Book.Image.Description,
                ImageTitle = post.Book.Image.Title,
                Lables = post.Labels.Select(labelPost => labelPost.Label).ToList(),
                LikeCount = post.Like,
                PostBody = post.Body,
                PostStatus = post.Status,
                UserId = post.User.Id,
                UserName = post.User.UserName,
                VisitedCount = post.VisitedNumber,
                PostTitle = post.Title,
                PostDescription = post.Description,
                PostKeywords = post.Keyword,
                ModifiedDate = post.ModifiedDate
            }).FirstOrDefault();
        }


        public int Count
        {
            get { return _posts.Count(); }
        }


        public void IncrementVisitedNumber(int id)
        {
            _posts.Find(id).VisitedNumber += 1;
        }


        public bool IsUserLikePost(int postId, int userId)
        {
            return _posts.Where(post => post.Id == postId).Any(post =>
                post.LikedUsers.Any(likedUser => likedUser.UserId == userId)
            );
        }


        public bool IsUserLikePost(int postId, string userName)
        {
            return _posts.Where(post => post.Id == postId).Any(post =>
                post.LikedUsers.Any(likedUser => likedUser.User.UserName == userName)
            );
        }

        public int Like(int id, User user)
        {
            var selectedPost = _posts.Find(id);
            selectedPost.Like += 1;

            _uow.Set<LikeUsersPost>().Add(new LikeUsersPost
            {
                User = user,
                Post = selectedPost
            });

            return selectedPost.Like;
        }

        public int DisLike(int id, User user)
        {
            var selectedPost = _posts.Find(id);
            selectedPost.Like -= 1;

            _uow.Set<LikeUsersPost>().Add(new LikeUsersPost
            {
                User = user,
                Post = selectedPost
            });

            return selectedPost.Like;
        }


        public Post Find(int id)
        {
            return _posts.Find(id);
        }


        public bool GetCommentStatus(int postId)
        {
            return _posts.Find(postId).CommentStatus ?? false;
        }

        public IList<RssPostModel> GetRssPosts(int count)
        {
            return _posts.AsNoTracking()
                .Select(post => new RssPostModel
                {
                    Body = post.Body,
                    CreatedDate = post.CreatedDate,
                    Id = post.Id,
                    Title = post.Title,
                    Author = post.User.UserName
                })
                .OrderByDescending(post => post.CreatedDate)
                .Take(count)
                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                .ToList();
        }

        public IList<PostDetailModel> GetUserPosts(string userName, int page, int count)
        {
            return
                _posts.AsNoTracking().Where(post => post.User.UserName == userName).Select(post => new PostDetailModel
                {
                    CommnetCount = post.Comments.Count,
                    Id = post.Id,
                    LikeCount = post.Like,
                    PostedDate = post.CreatedDate,
                    Title = post.Title,
                    VisitedCount = post.VisitedNumber
                }).OrderByDescending(post => post.PostedDate).Skip(page * count).Take(count).ToList();
        }

        public int GetUserPostsCount(string userName)
        {
            return _posts.Count(post => post.User.UserName == userName);
        }

        public IList<SiteMapModel> GetSiteMapData(int count)
        {
            return _posts.AsNoTracking()
                .OrderByDescending(post => post.CreatedDate)
                .Take(count)
                .Select(post => new SiteMapModel
                {
                    Id = post.Id,
                    CreatedDate = post.CreatedDate,
                    ModifiedDate = post.ModifiedDate,
                    Title = post.Title
                })
                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                .ToList();
        }

        #region Get Posts Data

        public Post GetPostById(int id)
        {
            return _posts.Find(id);
        }


        public IList<Post> GetAllPosts()
        {
            return _posts.ToList();
        }

        public IList<Post> GetPosts(Func<Post, bool> expression)
        {
            return _posts.Where(expression).ToList();
        }

        public Post GetPostDataById(int id)
        {
            return _posts.Where(x => x.Id == id).Include(x => x.Book).Include(x => x.Comments)
                .Include(x => x.User).FirstOrDefault();
        }

        #endregion

        #region Get Post Status

        public PostStatus GetPostStatusById(long id)
        {
            const PostStatus status = PostStatus.Visible;
            //if (selectedPost.Status == "visible")
            //{
            //    status = PostStatus.Visible;
            //}
            //else if (selectedPost.Status == "hidden")
            //{
            //    status = PostStatus.Hidden;
            //}
            //else if (selectedPost.Status == "draft")
            //{
            //    status = PostStatus.Draft;
            //}
            return status;
        }

        public IList<PostDataTableModel> GetPostDataTable(int page, int count, Order order = Order.Asscending,
            PostOrderBy orderBy = PostOrderBy.ById)
        {
            IQueryable<PostDataTableModel> lstPosts =
                _posts.Include(post => post.User)
                    .Include(post => post.Labels)
                    .Select(
                        post =>
                            new PostDataTableModel
                            {
                                CommentStatus = post.CommentStatus,
                                PostAuthor = post.User.UserName,
                                Title = post.Title,
                                labels = post.Labels.Select(labelPost => labelPost.Label).ToList(),
                                Status = post.Status,
                                VisitedNumber = post.VisitedNumber,
                                Id = post.Id
                            })
                    .AsQueryable();

            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case PostOrderBy.ById:
                        lstPosts = lstPosts.OrderBy(post => post.Id).AsQueryable();
                        break;
                    case PostOrderBy.ByTitle:
                        lstPosts = lstPosts.OrderBy(post => post.Title).AsQueryable();
                        break;
                    case PostOrderBy.ByPostAuthor:
                        lstPosts = lstPosts.OrderBy(post => post.PostAuthor).AsQueryable();
                        break;
                    case PostOrderBy.ByVisitedNumber:
                        lstPosts = lstPosts.OrderBy(post => post.VisitedNumber).AsQueryable();
                        break;
                    case PostOrderBy.ByLabels:
                        lstPosts = lstPosts.OrderBy(post => post.labels).AsQueryable();
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case PostOrderBy.ById:
                        lstPosts = lstPosts.OrderByDescending(post => post.Id).AsQueryable();
                        break;
                    case PostOrderBy.ByTitle:
                        lstPosts = lstPosts.OrderByDescending(post => post.Title).AsQueryable();
                        break;
                    case PostOrderBy.ByPostAuthor:
                        lstPosts = lstPosts.OrderByDescending(post => post.PostAuthor).AsQueryable();
                        break;
                    case PostOrderBy.ByVisitedNumber:
                        lstPosts = lstPosts.OrderByDescending(post => post.VisitedNumber).AsQueryable();
                        break;
                    case PostOrderBy.ByLabels:
                        lstPosts = lstPosts.OrderByDescending(post => post.labels).AsQueryable();
                        break;
                }
            }

            return lstPosts.ToList();
        }


        public int GetPostsCount()
        {
            return _posts.Count();
        }

        #endregion
    }
}
