using System;
using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime AddedDate { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual int LikeCount { get; set; }
        public virtual User User { get; set; }
        public virtual AnonymousUser AnonymousUser { get; set; }
        public virtual Post Post { get; set; }
        public virtual Page Page { get; set; }
        public virtual Article Article { get; set; }
        public virtual int? ParentId { get; set; }
        public virtual Comment Parent { get; set; }
        public ICollection<Comment> Children { get; set; }
        public virtual ICollection<User> LikedUsers { get; set; }
        public virtual byte[] RowVersion { get; set; }
    }
}