using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(e => e.Id, "IX_Id");

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Author).HasMaxLength(500);

            builder.Property(e => e.ISBN)
                .HasMaxLength(30)
                .HasColumnName("ISBN");

            builder.Property(e => e.Language).HasMaxLength(20);

            builder.Property(e => e.Name).HasMaxLength(200);

            builder.Property(e => e.Page).HasMaxLength(10);

            builder.Property(e => e.Publisher).HasMaxLength(50);

            builder.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.Property(e => e.Year).HasMaxLength(20);

            builder.HasOne(d => d.Post)
                .WithOne(p => p.Book)
                .HasForeignKey<Book>(d => d.Id)
                .HasConstraintName("FK_dbo.Books_dbo.Posts_Id");
        }
    }
}