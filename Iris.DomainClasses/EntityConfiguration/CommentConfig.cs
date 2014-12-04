using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class CommentConfig : EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            HasOptional(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.Post).WithMany(x => x.Comments).WillCascadeOnDelete();
            HasOptional(x => x.Page).WithMany(x => x.Comments).WillCascadeOnDelete();
            HasOptional(x => x.Article).WithMany(x => x.Comments).WillCascadeOnDelete();

            //this.HasOptional(x => x.AnonymousUser).WithRequired(x => x.Comment).WillCascadeOnDelete(true);
            HasMany(x => x.LikedUsers).WithMany(x => x.LikedComments).Map(x =>
            {
                x.ToTable("LikeUsersComments");
                x.MapLeftKey("CommentId");
                x.MapRightKey("UserId");
            });

            Property(x => x.Body).HasMaxLength(4000);
            Property(x => x.RowVersion).IsRowVersion();
        }
    }
}