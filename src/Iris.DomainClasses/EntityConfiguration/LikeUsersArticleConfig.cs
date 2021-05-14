using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LikeUsersArticleConfig : IEntityTypeConfiguration<LikeUsersArticle>
    {
        public void Configure(EntityTypeBuilder<LikeUsersArticle> builder)
        {
            builder.HasKey(e => new { e.ArticleId, e.UserId })
                .HasName("PK_dbo.LikeUsersArticles");

            builder.HasIndex(e => e.ArticleId, "IX_ArticleId");

            builder.HasIndex(e => e.UserId, "IX_UserId");

            builder.HasOne(d => d.Article)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_dbo.LikeUsersArticles_dbo.Articles_ArticleId");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikeUsersArticles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.LikeUsersArticles_dbo.Users_UserId");
        }
    }
}
