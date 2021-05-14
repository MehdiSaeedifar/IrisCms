using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class AnonymousUserConfig : IEntityTypeConfiguration<AnonymousUser>
    {
        public void Configure(EntityTypeBuilder<AnonymousUser> builder)
        {
            builder.Property(e => e.Email).HasMaxLength(50);

            builder.Property(e => e.IP)
                .HasMaxLength(50)
                .HasColumnName("IP");

            builder.Property(e => e.Name).HasMaxLength(20);
        }
    }
}
