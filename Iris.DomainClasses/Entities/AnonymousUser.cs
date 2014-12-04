using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class AnonymousUser
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string IP { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}