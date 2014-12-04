using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class MessageConfig : EntityTypeConfiguration<Message>
    {
        public MessageConfig()
        {
            Property(x => x.Body).IsMaxLength();
            Property(m => m.Subject).HasMaxLength(500);
        }
    }
}