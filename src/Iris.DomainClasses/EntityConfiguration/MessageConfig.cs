using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.AddedDate).HasColumnType("datetime");

            builder.Property(e => e.Subject).HasMaxLength(500);

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.Messages_dbo.Users_User_Id");
        }
    }
}
