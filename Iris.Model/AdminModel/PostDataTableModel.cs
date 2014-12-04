using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Model.AdminModel
{
    public class PostDataTableModel
    {
        public int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Status { get; set; }
        public virtual bool? CommentStatus { get; set; }
        public virtual string PostAuthor { get; set; }
        public virtual int VisitedNumber { get; set; }
        public virtual ICollection<Label> labels { get; set; }
    }
}