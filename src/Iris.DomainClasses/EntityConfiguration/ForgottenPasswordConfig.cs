using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class ForgottenPasswordConfig : IEntityTypeConfiguration<ForgottenPassword>
    {
        public void Configure(EntityTypeBuilder<ForgottenPassword> builder)
        {
            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.Key).HasMaxLength(40);

            builder.Property(e => e.ResetDateTime).HasColumnType("datetime");

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.ForgottenPasswords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.ForgottenPasswords_dbo.Users_User_Id");
        }
    }
}
