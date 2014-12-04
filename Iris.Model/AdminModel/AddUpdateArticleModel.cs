using System.Web.Mvc;

namespace Iris.Model.AdminModel
{
    public class AddUpdateArticleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public string Keywords { get; set; }
        public string Description { get; set; }
        public bool CommentStatus { get; set; }
        public string ArticleStatus { get; set; }
        public int CategoryId { get; set; }
    }
}