using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class PageConfig : EntityTypeConfiguration<Page>
    {
        public PageConfig()
        {
            // Self reference entity
            HasOptional(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId);
            HasOptional(x => x.EditedByUser).WithMany(x => x.Pages).WillCascadeOnDelete(false);
            HasMany(x => x.LikedUsers).WithMany(x => x.LikedPages).Map(x =>
            {
                x.ToTable("LikeUsersPages");
                x.MapLeftKey("PageId");
                x.MapRightKey("UserId");
            });
            Property(x => x.Body).IsMaxLength();
            Property(x => x.Description).HasMaxLength(400);
            Property(x => x.Keyword).HasMaxLength(100);
            Property(x => x.Status).HasMaxLength(10);
            Property(x => x.Title).HasMaxLength(200);
        }
    }
}