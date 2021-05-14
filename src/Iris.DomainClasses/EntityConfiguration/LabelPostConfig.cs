using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.DomainClasses.EntityConfiguration
{
    public class LabelPostConfig : IEntityTypeConfiguration<LabelPost>
    {
        public void Configure(EntityTypeBuilder<LabelPost> builder)
        {
            builder.HasKey(e => new { e.LabelId, e.PostId })
                .HasName("PK_dbo.LabelPosts");

            builder.HasIndex(e => e.LabelId, "IX_Label_Id");

            builder.HasIndex(e => e.PostId, "IX_Post_Id");

            builder.Property(e => e.LabelId).HasColumnName("Label_Id");

            builder.Property(e => e.PostId).HasColumnName("Post_Id");

            builder.HasOne(d => d.Label)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.LabelId)
                .HasConstraintName("FK_dbo.LabelPosts_dbo.Labels_Label_Id");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.Labels)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_dbo.LabelPosts_dbo.Posts_Post_Id");
        }
    }
}
