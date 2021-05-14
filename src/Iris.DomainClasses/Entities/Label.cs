using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Label
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<LabelPost> Posts { get; set; }
    }
}
