using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class UserMetaDataConfig : IEntityTypeConfiguration<UserMetaData>
    {
        public void Configure(EntityTypeBuilder<UserMetaData> builder)
        {
            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.AvatarPath).HasMaxLength(200);

            builder.Property(e => e.BirthDay).HasColumnType("datetime");

            builder.Property(e => e.Description).HasMaxLength(1000);

            builder.Property(e => e.FirstName).HasMaxLength(50);

            builder.Property(e => e.LastName).HasMaxLength(50);

            builder.Property(e => e.Major).HasMaxLength(30);

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.User)
                .WithMany("UserMetaData")
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.UserMetaDatas_dbo.Users_User_Id");
        }
    }
}
