using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.RoleId, "IX_Role_Id");

            builder.Property(e => e.BanedDate).HasColumnType("datetime");

            builder.Property(e => e.CreatedDate).HasColumnType("datetime");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.IP)
                .HasMaxLength(20)
                .HasColumnName("IP");

            builder.Property(e => e.LastActivity).HasColumnType("datetime");

            builder.Property(e => e.LastLoginDate).HasColumnType("datetime");

            builder.Property(e => e.LastPasswordChange).HasColumnType("datetime");

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.RoleId).HasColumnName("Role_Id");

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_dbo.Users_dbo.Roles_Role_Id");
        }
    }
}
