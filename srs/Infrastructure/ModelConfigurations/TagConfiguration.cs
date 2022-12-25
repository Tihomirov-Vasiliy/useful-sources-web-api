using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.ModelConfigurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired();

            //many to many relationships between tag and parent tag
            builder
                .HasMany(t => t.ParentTags)
                .WithMany(t => t.TagsOf)
                .UsingEntity<Dictionary<string, object>>(
                    "tag_parenttag",
                    j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("parenttag_id")
                    .HasConstraintName("FK_tagparenttag_parenttags_parenttagid")
                    .OnDelete(DeleteBehavior.Cascade),
                     j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("tag_id")
                    .HasConstraintName("FK_tagparenttag_tags_tagid")
                    .OnDelete(DeleteBehavior.Cascade));

            builder
                .ToTable("tags");
        }
    }
}
