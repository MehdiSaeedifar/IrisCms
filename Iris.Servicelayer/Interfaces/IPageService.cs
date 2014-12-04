using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Servicelayer.Interfaces
{
    public class PageDropDownList
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public interface IPageService
    {
        int Count { get; }
        void Add(Page page);
        Page Find(int id);
        IList<Page> GetAll();
        IList<PageDropDownList> DropDownListData();
        IList<PageDropDownList> DropDownListData(int id);

        IList<PageDataTableModel> GetDataTable(string term, int page, int count,
            Order order, PageOrderBy orderBy, PageSearchBy searchBy);

        void Remove(int id);
        EditPageModel GetEditingData(int id);
        void Update(Page page);
        IList<Page> GetNavBarData(Func<Page, bool> expression);
        bool GetCommentStatus(int id);
        bool IsUserLikePage(int id, string userName);
        int Like(int id, User user);
        int DisLike(int id, User user);
        void IncrementVisitedCount(int id);
    }
}