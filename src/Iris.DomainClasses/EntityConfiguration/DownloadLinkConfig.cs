using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class DownloadLinkConfig : IEntityTypeConfiguration<DownloadLink>
    {
        public void Configure(EntityTypeBuilder<DownloadLink> builder)
        {
            builder.HasIndex(e => e.PostId, "IX_Post_Id");

            builder.Property(e => e.FileFormat).HasMaxLength(50);

            builder.Property(e => e.FileSize).HasMaxLength(50);

            builder.Property(e => e.Link).HasMaxLength(1000);

            builder.Property(e => e.PostId).HasColumnName("Post_Id");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.DownloadLinks)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo.DownloadLinks_dbo.Posts_Post_Id");
        }
    }
}
