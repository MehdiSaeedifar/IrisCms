using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class MessageAnswareConfig : IEntityTypeConfiguration<MessageAnsware>
    {
        public void Configure(EntityTypeBuilder<MessageAnsware> builder)
        {
            builder.HasIndex(e => e.MessageId, "IX_Message_Id");

            builder.HasIndex(e => e.UserId, "IX_User_Id");

            builder.Property(e => e.AnswaredDate).HasColumnType("datetime");

            builder.Property(e => e.MessageId).HasColumnName("Message_Id");

            builder.Property(e => e.UserId).HasColumnName("User_Id");

            builder.HasOne(d => d.Message)
                .WithMany(p => p.Answares)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_dbo.MessageAnswares_dbo.Messages_Message_Id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.MessageAnswares)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.MessageAnswares_dbo.Users_User_Id");
        }
    }
}
