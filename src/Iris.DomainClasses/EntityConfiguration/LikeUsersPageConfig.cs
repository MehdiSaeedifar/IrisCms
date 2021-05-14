using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LikeUsersPageConfig : IEntityTypeConfiguration<LikeUsersPage>
    {
        public void Configure(EntityTypeBuilder<LikeUsersPage> builder)
        {
            builder.HasKey(e => new { e.PageId, e.UserId })
                .HasName("PK_dbo.LikeUsersPages");

            builder.HasIndex(e => e.PageId, "IX_PageId");

            builder.HasIndex(e => e.UserId, "IX_UserId");

            builder.HasOne(d => d.Page)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(d => d.PageId)
                .HasConstraintName("FK_dbo.LikeUsersPages_dbo.Pages_PageId");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikeUsersPages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.LikeUsersPages_dbo.Users_UserId");
        }
    }
}
