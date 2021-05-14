using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}