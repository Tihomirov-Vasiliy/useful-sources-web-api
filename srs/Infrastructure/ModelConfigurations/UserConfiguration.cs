using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.Login)
                .IsRequired();

            builder
                .Property(u => u.Password)
                .IsRequired();

            builder
                .Property(u => u.Role)
                .IsRequired();

            builder.ToTable("users");
        }
    }
}
