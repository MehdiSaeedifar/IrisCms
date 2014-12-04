using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class BookImageConfig : EntityTypeConfiguration<BookImage>
    {
        public BookImageConfig()
        {
            HasRequired(x => x.Book).WithOptional(x => x.Image).WillCascadeOnDelete(true);
            Property(x => x.Description).HasMaxLength(100);
            Property(x => x.Title).HasMaxLength(200);
            Property(x => x.Path).HasMaxLength(400);
        }
    }
}