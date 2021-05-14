using System;
using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public enum PostStatus
    {
        Hidden,
        Visible,
        Draft,
    }

    public class Page
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }

        public virtual string Body { get; set; }

        public virtual string Keyword { get; set; }

        public virtual string Description { get; set; }

        public virtual string Status { get; set; }

        public virtual bool CommentStatus { get; set; }

        public virtual int VisitedCount { get; set; }

        public virtual int LikeCount { get; set; }

        public virtual int? Order { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public int? EditedByUserId { get; set; }

        public virtual User EditedByUser { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual int? ParentId { get; set; }

        public virtual Page Parent { get; set; }

        public virtual ICollection<Page> Children { get; set; }

        public virtual ICollection<LikeUsersPage> LikedUsers { get; set; }
    }
}
