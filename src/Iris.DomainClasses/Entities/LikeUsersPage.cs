namespace Iris.DomainClasses.Entities
{
    public class LikeUsersPage
    {
        public int PageId { get; set; }

        public virtual Page Page { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
