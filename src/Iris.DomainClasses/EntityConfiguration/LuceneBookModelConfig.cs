using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LuceneBookModelConfig : IEntityTypeConfiguration<LuceneBookModel>
    {
        public void Configure(EntityTypeBuilder<LuceneBookModel> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
