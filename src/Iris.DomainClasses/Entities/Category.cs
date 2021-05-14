using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Category
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int? Order { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
