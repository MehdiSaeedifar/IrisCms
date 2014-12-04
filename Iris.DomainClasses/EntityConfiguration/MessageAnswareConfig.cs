using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class MessageAnswareConfig : EntityTypeConfiguration<MessageAnsware>
    {
        public MessageAnswareConfig()
        {
            Property(x => x.Body).IsMaxLength();
        }
    }
}