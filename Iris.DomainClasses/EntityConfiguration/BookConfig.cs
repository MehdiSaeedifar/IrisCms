using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class BookConfig : EntityTypeConfiguration<Book>
    {
        public BookConfig()
        {
            HasOptional(x => x.Image).WithRequired(x => x.Book).WillCascadeOnDelete(true);
            Property(x => x.Author).HasMaxLength(500);
            Property(x => x.Description).IsMaxLength();
            Property(x => x.ISBN).HasMaxLength(30);
            Property(x => x.Language).HasMaxLength(20);
            Property(x => x.Name).HasMaxLength(200);
            Property(x => x.Page).HasMaxLength(10);
            Property(x => x.RowVersion).IsRowVersion();
            Property(x => x.Year).HasMaxLength(20);
            Property(x => x.Publisher).HasMaxLength(50);
        }
    }
}