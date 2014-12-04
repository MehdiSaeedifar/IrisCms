using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class ForgottenPasswordConfig : EntityTypeConfiguration<ForgottenPassword>
    {
        public ForgottenPasswordConfig()
        {
            Property(x => x.Key).HasMaxLength(40);
        }
    }
}