using System.Collections.Generic;

namespace Iris.DomainClasses.Entities
{
    public class Book
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        // TODO
        //[AllowHtml]
        public virtual string Description { get; set; }

        public virtual string Author { get; set; }

        public virtual string ISBN { get; set; }

        public virtual string Year { get; set; }

        public virtual string Page { get; set; }

        public virtual string Language { get; set; }

        public virtual string Publisher { get; set; }

        public virtual Post Post { get; set; }

        public BookImage Image { get; set; }

        public virtual byte[] RowVersion { get; set; }
    }
}
