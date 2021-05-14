using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LikeUsersPostConfig : IEntityTypeConfiguration<LikeUsersPost>
    {
        public void Configure(EntityTypeBuilder<LikeUsersPost> builder)
        {
            builder.HasKey(e => new { e.PostId, e.UserId })
                .HasName("PK_dbo.LikeUsersPosts");

            builder.HasIndex(e => e.PostId, "IX_PostId");

            builder.HasIndex(e => e.UserId, "IX_UserId");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_dbo.LikeUsersPosts_dbo.Posts_PostId");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikeUsersPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.LikeUsersPosts_dbo.Users_UserId");
        }
    }
}
