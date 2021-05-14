using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class AutoCompleteSearchBookModelConfig : IEntityTypeConfiguration<AutoCompleteSearchBookModel>
    {
        public void Configure(EntityTypeBuilder<AutoCompleteSearchBookModel> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
