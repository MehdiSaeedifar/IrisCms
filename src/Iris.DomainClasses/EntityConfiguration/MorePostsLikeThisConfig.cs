using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class MorePostsLikeThisConfig : IEntityTypeConfiguration<MorePostsLikeThis>
    {
        public void Configure(EntityTypeBuilder<MorePostsLikeThis> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
