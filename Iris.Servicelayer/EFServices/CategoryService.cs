using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Model.SideBar;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Caching;

namespace Iris.Servicelayer.EFServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbSet<Category> _categories;

        public CategoryService(IUnitOfWork uow)
        {
            _categories = uow.Set<Category>();
        }

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public bool IsExistByName(string name)
        {
            return _categories.Any(category => category.Name == name);
        }

        public void Remove(int id)
        {
            _categories.Remove(_categories.Find(id));
        }

        public void Update(Category category)
        {
            Category selectedCategory = _categories.Find(category.Id);
            selectedCategory.Name = category.Name;
            selectedCategory.Description = category.Description;
            selectedCategory.Order = category.Order;
        }

        public Category Find(int id)
        {
            return _categories.Find(id);
        }

        public Category Find(string categoryName)
        {
            return _categories.FirstOrDefault(category => category.Name == categoryName);
        }


        public IList<CategoryDataTableModel> GetDataTable(string term, int page, int count,
            Order order, CategoryOrderBy orderBy, CategorySearchBy searchBy)
        {
            IQueryable<Category> selectedCategories = _categories.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case CategorySearchBy.Name:
                        selectedCategories =
                            selectedCategories.Where(category => category.Name.Contains(term)).AsQueryable();
                        break;
                    case CategorySearchBy.Description:
                        selectedCategories =
                            selectedCategories.Where(category => category.Description.Contains(term)).AsQueryable();
                        break;
                }
            }

            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case CategoryOrderBy.Id:
                        selectedCategories = selectedCategories.OrderBy(category => category.Id).AsQueryable();
                        break;
                    case CategoryOrderBy.Name:
                        selectedCategories = selectedCategories.OrderBy(category => category.Name).AsQueryable();
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case CategoryOrderBy.Id:
                        selectedCategories = selectedCategories.OrderByDescending(category => category.Id).AsQueryable();
                        break;
                    case CategoryOrderBy.Name:
                        selectedCategories =
                            selectedCategories.OrderByDescending(category => category.Name).AsQueryable();
                        break;
                }
            }

            return selectedCategories.Select(category => new CategoryDataTableModel
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                ArticlesCount = category.Articles.Count,
                Order = category.Order,
            }).Skip(page * count).Take(count).ToList();
        }


        public int Count
        {
            get { return _categories.Count(); }
        }


        public int GetMaxOrder()
        {
            return _categories.Max(category => category.Order) ?? 0;
        }


        public bool IsExistByOrder(int orderNumber)
        {
            return _categories.Any(category => category.Order == orderNumber);
        }

        public IList<Category> GetAll()
        {
            return _categories.AsNoTracking().ToList();
        }

        [CacheMethod(SecondsToCache = 120)]
        public IList<Category> GetAnnounceData(int count)
        {
            return
                _categories.AsNoTracking().Include(category => category.Articles).Where(category => category.Order < 1)
                    .OrderBy(category => category.Order).Take(count)
                    .ToList();
        }


        [CacheMethod(SecondsToCache = 120)]
        public IList<CategoryModel> GetSideBarData(int articleCount)
        {
            return
                _categories.AsNoTracking().Include(category => category.Articles)
                    .Where(category => category.Order > 0)
                    .OrderBy(category => category.Order)
                    .Select(category => new CategoryModel
                    {
                        articles = category.Articles.Select(x => new ArticleSideBarModel { ArticleId = x.Id, ArticleTitle = x.Title }),
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        CategoryDescription = category.Description
                    }).Take(articleCount).ToList();
        }

    }
}