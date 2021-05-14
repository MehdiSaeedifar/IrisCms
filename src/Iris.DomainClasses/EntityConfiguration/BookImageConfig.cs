using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class BookImageConfig : IEntityTypeConfiguration<BookImage>
    {
        public void Configure(EntityTypeBuilder<BookImage> builder)
        {
            builder.HasIndex(e => e.BookId, "IX_Book_Id");

            builder.Property(e => e.BookId).HasColumnName("Book_Id");

            builder.Property(e => e.Description).HasMaxLength(100);

            builder.Property(e => e.Path).HasMaxLength(400);

            builder.Property(e => e.Title).HasMaxLength(200);

            builder.Property(e => e.UploadedDate).HasColumnType("datetime");

            builder.HasOne(d => d.Book)
                .WithMany("Image")
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_dbo.BookImages_dbo.Books_Book_Id");
        }
    }
}
