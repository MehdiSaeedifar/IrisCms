using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasIndex(e => e.AnonymousUserId, "IX_AnonymousUser_Id");

            builder.HasIndex(e => e.ArticleId, "IX_Article_Id");

            builder.HasIndex(e => e.PageId, "IX_Page_Id");

            builder.HasIndex(e => e.ParentId, "IX_ParentId");

            builder.HasIndex(e => e.PostId, "IX_Post_Id");

            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.AddedDate).HasColumnType("datetime");

            builder.Property(e => e.AnonymousUserId).HasColumnName("AnonymousUser_Id");

            builder.Property(e => e.ArticleId).HasColumnName("Article_Id");

            builder.Property(e => e.Body).HasMaxLength(4000);

            builder.Property(e => e.PageId).HasColumnName("Page_Id");

            builder.Property(e => e.PostId).HasColumnName("Post_Id");

            builder.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.AnonymousUser)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.AnonymousUserId)
                .HasConstraintName("FK_dbo.Comments_dbo.AnonymousUsers_AnonymousUser_Id");

            builder.HasOne(d => d.Article)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo.Comments_dbo.Articles_Article_Id");

            builder.HasOne(d => d.Page)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo.Comments_dbo.Pages_Page_Id");

            builder.HasOne(d => d.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_dbo.Comments_dbo.Comments_ParentId");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo.Comments_dbo.Posts_Post_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.Comments_dbo.Users_User_Id");
        }
    }
}
