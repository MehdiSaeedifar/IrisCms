using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasIndex(e => e.EditedByUserId, "IX_EditedByUser_Id");

            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.CreatedDate).HasColumnType("datetime");

            builder.Property(e => e.Description).HasMaxLength(400);

            builder.Property(e => e.EditedByUserId).HasColumnName("EditedByUser_Id");

            builder.Property(e => e.Keyword).HasMaxLength(500);

            builder.Property(e => e.ModifiedDate).HasColumnType("datetime");

            builder.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.Property(e => e.Status).HasMaxLength(50);

            builder.Property(e => e.Title).HasMaxLength(200);

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.Status);

            builder.HasOne(d => d.EditedByUser)
                .WithMany(p => p.PostEditedByUsers)
                .HasForeignKey(d => d.EditedByUserId)
                .HasConstraintName("FK_dbo.Posts_dbo.Users_EditedByUser_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Posts_dbo.Users_User_Id");
        }
    }
}
