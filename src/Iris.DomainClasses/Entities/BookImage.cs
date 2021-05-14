using System;

namespace Iris.DomainClasses.Entities
{
    public class BookImage
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Path { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? UploadedDate { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
