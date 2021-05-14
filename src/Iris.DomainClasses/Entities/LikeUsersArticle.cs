namespace Iris.DomainClasses.Entities
{
    public class LikeUsersArticle
    {
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int UserId { get; set; }
        
        public virtual User User { get; set; }
    }
}
