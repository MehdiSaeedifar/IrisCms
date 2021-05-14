using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasIndex(e => e.CategoryId, "IX_Category_Id");
            
            builder.HasIndex(e => e.EditedByUserId, "IX_EditedByUser_Id");
            
            builder.HasIndex(e => e.UserId, "IX_User_Id");
            
            builder.Property(e => e.CategoryId).HasColumnName("Category_Id");
            
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            
            builder.Property(e => e.Description).HasMaxLength(400);
            
            builder.Property(e => e.EditedByUserId).HasColumnName("EditedByUser_Id");
            
            builder.Property(e => e.Keyword).HasMaxLength(100);
            
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime");
            
            builder.Property(e => e.Status).HasMaxLength(10);
            
            builder.Property(e => e.Title).HasMaxLength(200);
            
            builder.Property(e => e.UserId).HasColumnName("User_Id");
            
            builder.HasOne(d => d.Category)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_dbo.Articles_dbo.Categories_Category_Id");

            builder.HasOne(d => d.EditedByUser)
                .WithMany(p => p.ArticleEditedByUsers)
                .HasForeignKey(d => d.EditedByUserId)
                .HasConstraintName("FK_dbo.Articles_dbo.Users_EditedByUser_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.Articles_dbo.Users_User_Id");
        }
    }
}