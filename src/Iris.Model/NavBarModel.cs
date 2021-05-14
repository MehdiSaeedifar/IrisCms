using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Model
{
    public class NavBarModel
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Status { get; set; }
        public virtual int? Order { get; set; }
        public virtual NavBarModel Parent { get; set; }
        public virtual ICollection<Page> Children { get; set; }
    }
}