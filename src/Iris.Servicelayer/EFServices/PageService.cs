using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EFCoreSecondLevelCacheInterceptor;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Page> _pages;

        public PageService(IUnitOfWork uow)
        {
            _uow = uow;
            _pages = uow.Set<Page>();
        }

        public void Add(Page page)
        {
            _pages.Add(page);
        }


        public Page Find(int id)
        {
            return _pages
                .Include(page => page.User)
                .Include(page => page.Comments)
                .FirstOrDefault(page => page.Id == id);
        }


        public IList<Page> GetAll()
        {
            return _pages.ToList();
        }


        public IList<PageDropDownList> DropDownListData()
        {
            return
                _pages.AsNoTracking().Select(page => new PageDropDownList { Id = page.Id, Title = page.Title }).ToList();
        }

        public IList<PageDropDownList> DropDownListData(int id)
        {
            return _pages.AsNoTracking().Where(page => page.Id != id)
                .Select(page => new PageDropDownList { Id = page.Id, Title = page.Title }).ToList();
        }

        public IList<PageDataTableModel> GetDataTable(string term, int page, int count,
            Order order, PageOrderBy orderBy, PageSearchBy searchBy)
        {
            IQueryable<Page> selectedPages =
                _pages.Include(Page => Page.Comments).Include(Page => Page.User).AsQueryable();

            // page in lambda expression changed to Page, because it confilicts with page Parameter
            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case PageSearchBy.Title:
                        selectedPages = selectedPages.Where(Page => Page.Title.Contains(term)).AsQueryable();
                        break;
                    case PageSearchBy.UserName:
                        selectedPages = selectedPages.Where(Page => Page.User.UserName.Contains(term)).AsQueryable();
                        break;
                    case PageSearchBy.ParentTitle:
                        selectedPages = selectedPages.Where(Page => Page.Parent.Title.Contains(term)).AsQueryable();
                        break;
                    default:
                        break;
                }
            }

            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case PageOrderBy.Title:
                        selectedPages = selectedPages.OrderBy(Page => Page.Title).AsQueryable();
                        break;
                    case PageOrderBy.Date:
                        selectedPages = selectedPages.OrderBy(Page => Page.CreatedDate).AsQueryable();
                        break;
                    case PageOrderBy.CommentCount:
                        selectedPages = selectedPages.OrderBy(Page => Page.Comments.Count).AsQueryable();
                        break;
                    case PageOrderBy.Status:
                        break;
                    case PageOrderBy.UserName:
                        selectedPages = selectedPages.OrderBy(Page => Page.User.UserName).AsQueryable();
                        break;
                    case PageOrderBy.ParentTitle:
                        selectedPages =
                            selectedPages.OrderBy(Page => Page.CreatedDate).OrderBy(Page => Page.Parent.Title)
                                .AsQueryable();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case PageOrderBy.Title:
                        selectedPages = selectedPages.OrderByDescending(Page => Page.Title).AsQueryable();
                        break;
                    case PageOrderBy.Date:
                        selectedPages = selectedPages.OrderByDescending(Page => Page.CreatedDate).AsQueryable();
                        break;
                    case PageOrderBy.CommentCount:
                        selectedPages = selectedPages.OrderByDescending(Page => Page.Comments.Count).AsQueryable();
                        break;
                    case PageOrderBy.Status:
                        break;
                    case PageOrderBy.UserName:
                        selectedPages = selectedPages.OrderByDescending(Page => Page.User.UserName).AsQueryable();
                        break;
                    case PageOrderBy.ParentTitle:
                        selectedPages =
                            selectedPages.OrderByDescending(Page => Page.CreatedDate).OrderBy(Page => Page.Parent.Title)
                                .AsQueryable();
                        break;
                    default:
                        break;
                }
            }

            return selectedPages.Select(Page => new PageDataTableModel
            {
                CommentCount = Page.Comments.Count,
                CommentStatus = Page.CommentStatus,
                CreatedDate = Page.CreatedDate,
                Id = Page.Id,
                ParentId = Page.Parent.Id,
                ParentTitle = Page.Parent.Title,
                Status = Page.Status,
                Title = Page.Title,
                UserId = Page.User.Id,
                UserName = Page.User.UserName
            }).Skip(page * count).Take(count).ToList();
        }


        public int Count
        {
            get { return _pages.Count(); }
        }


        public void Remove(int id)
        {
            _pages.Remove(_pages.Find(id));
        }


        public EditPageModel GetEditingData(int id)
        {
            return _pages.Where(page => page.Id == id).Select(page =>
                new EditPageModel
                {
                    Body = page.Body,
                    CommentStatus = page.CommentStatus,
                    Description = page.Description,
                    Id = page.Id,
                    Keyword = page.Keyword,
                    Order = page.Order,
                    ParentId = page.Parent.Id,
                    Status = page.Status,
                    Title = page.Title
                }).FirstOrDefault();
        }


        public void Update(Page page)
        {
            Page selectedPage = _pages.Find(page.Id);
            selectedPage.Body = page.Body;
            selectedPage.CommentStatus = page.CommentStatus;
            selectedPage.Description = page.Description;
            selectedPage.Keyword = page.Keyword;
            selectedPage.ModifiedDate = page.ModifiedDate;
            selectedPage.Order = page.Order;
            selectedPage.Parent = page.Parent;
            selectedPage.Status = page.Status;
            selectedPage.Title = page.Title;
            selectedPage.EditedByUser = page.EditedByUser;
        }

        public IList<Page> GetNavBarData(Func<Page, bool> expression)
        {
            return
                _pages.AsNoTracking()
                    .Include(page => page.Children)
                    .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
                    .ToList()
                    .Where(page => page.Parent == null)
                    .ToList();
        }

        public bool GetCommentStatus(int id)
        {
            return _pages.Find(id).CommentStatus;
        }


        public bool IsUserLikePage(int id, string userName)
        {
            return _pages.Where(page => page.Id == id).Any(page =>
                page.LikedUsers.Any(likedUser => likedUser.User.UserName == userName)
            );
        }


        public int Like(int id, User user)
        {
            var selectedPage = _pages.Find(id);
            selectedPage.LikeCount += 1;

            _uow.Set<LikeUsersPage>().Add(new LikeUsersPage
            {
                User = user,
                Page = selectedPage
            });

            return selectedPage.LikeCount;
        }


        public int DisLike(int id, User user)
        {
            var selectedPage = _pages.Find(id);
            selectedPage.LikeCount -= 1;

            _uow.Set<LikeUsersPage>().Add(new LikeUsersPage
            {
                User = user,
                Page = selectedPage
            });

            return selectedPage.LikeCount;
        }


        public void IncrementVisitedCount(int id)
        {
            ((IrisDbContext)_uow).Database.ExecuteSqlRaw("/* Skip Invalidate Cache */UPDATE Pages SET VisitedCount += 1 WHERE Id ={0}", id);
        }
    }
}