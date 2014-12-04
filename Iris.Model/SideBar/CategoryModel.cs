using System.Collections.Generic;

namespace Iris.Model.SideBar
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public IEnumerable<ArticleSideBarModel> articles { get; set; }
    }

    public class ArticleSideBarModel
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
    }
}