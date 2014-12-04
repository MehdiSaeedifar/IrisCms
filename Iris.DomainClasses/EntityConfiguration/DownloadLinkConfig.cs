using System.Data.Entity.ModelConfiguration;
using Iris.DomainClasses.Entities;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class DownloadLinkConfig : EntityTypeConfiguration<DownloadLink>
    {
        public DownloadLinkConfig()
        {
            //this.HasOptional(x=>x.Post).WithMany(x => x.DownloadLinks).WillCascadeOnDelete();
            Property(entity => entity.Link).HasMaxLength(1000);
            Property(entity => entity.FileFormat).HasMaxLength(50);
            Property(entity => entity.FileSize).HasMaxLength(50);
        }
    }
}