using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class PageConfig : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasIndex(e => e.EditedByUserId, "IX_EditedByUser_Id");

            builder.HasIndex(e => e.ParentId, "IX_ParentId");

            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.CreatedDate).HasColumnType("datetime");

            builder.Property(e => e.Description).HasMaxLength(400);

            builder.Property(e => e.EditedByUserId).HasColumnName("EditedByUser_Id");

            builder.Property(e => e.Keyword).HasMaxLength(100);

            builder.Property(e => e.ModifiedDate).HasColumnType("datetime");

            builder.Property(e => e.Status).HasMaxLength(10);

            builder.Property(e => e.Title).HasMaxLength(200);

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.EditedByUser)
                .WithMany(p => p.PageEditedByUsers)
                .HasForeignKey(d => d.EditedByUserId)
                .HasConstraintName("FK_dbo.Pages_dbo.Users_EditedByUser_Id");

            builder.HasOne(d => d.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_dbo.Pages_dbo.Pages_ParentId");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Pages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.Pages_dbo.Users_User_Id");
        }
    }
}
