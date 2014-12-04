using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class CategoryConfig : EntityTypeConfiguration<Category>
    {
        public CategoryConfig()
        {
            Property(category => category.Name).HasMaxLength(100);
            Property(category => category.Description).HasMaxLength(1000);
        }
    }
}