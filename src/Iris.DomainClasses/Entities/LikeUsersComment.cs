namespace Iris.DomainClasses.Entities
{
    public class LikeUsersComment
    {
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
