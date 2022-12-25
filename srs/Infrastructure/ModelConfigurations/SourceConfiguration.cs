using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.ModelConfigurations
{
    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder
                .Property(s => s.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(s => s.Uri)
                .HasMaxLength(2048)
                .IsRequired();

            //many to many relationship between source and tag
            builder
                .HasMany(s => s.Tags)
                .WithMany(t => t.Sources)
                .UsingEntity<Dictionary<string, object>>(
                "source_tag",
                    j => j
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey("tag_id")
                .HasConstraintName("FK_sourcetag_tags_tagid")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade),
                    j => j
                .HasOne<Source>()
                .WithMany()
                .HasForeignKey("source_id")
                .HasConstraintName("FK_sourcetag_sources_sourceid")
                .OnDelete(DeleteBehavior.Cascade));

            builder
                .ToTable("sources");
        }
    }
}
