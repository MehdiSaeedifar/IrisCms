using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LikeUsersCommentConfig : IEntityTypeConfiguration<LikeUsersComment>
    {
        public void Configure(EntityTypeBuilder<LikeUsersComment> builder)
        {
            builder.HasKey(e => new { e.CommentId, e.UserId })
                .HasName("PK_dbo.LikeUsersComments");

            builder.HasIndex(e => e.CommentId, "IX_CommentId");

            builder.HasIndex(e => e.UserId, "IX_UserId");

            builder.HasOne(d => d.Comment)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_dbo.LikeUsersComments_dbo.Comments_CommentId");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikeUsersComments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.LikeUsersComments_dbo.Users_UserId");
        }
    }
}
