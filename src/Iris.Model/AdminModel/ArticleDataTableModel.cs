namespace Iris.Model.AdminModel
{
    public class ArticleDataTableModel
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ArticleStatus { get; set; }
        public bool CommentStatus { get; set; }
        public int VisitedCount { get; set; }
        public int LikeCount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}