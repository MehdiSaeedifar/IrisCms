using System.Collections.Generic;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Model.SideBar;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Servicelayer.Interfaces
{
    public interface ICategoryService
    {
        int Count { get; }
        void Add(Category category);
        bool IsExistByName(string name);
        void Remove(int id);
        void Update(Category category);
        Category Find(int id);
        Category Find(string categoryName);

        IList<CategoryDataTableModel> GetDataTable(string term, int page, int count,
            Order order, CategoryOrderBy orderBy, CategorySearchBy searchBy);

        int GetMaxOrder();
        bool IsExistByOrder(int orderNumber);
        IList<Category> GetAll();
        IList<Category> GetAnnounceData(int count);
        IList<CategoryModel> GetSideBarData(int articleCount = 10);
    }
}