using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Entities;
using Infrastructure.Interseptors;

namespace Infrastructure.Context
{
    public class UsefulSourcesContext : DbContext
    {
        private AuditableEntitySaveChangesInterseptor _auditableEntitySaveChangesInterseptor;

        public DbSet<Source> Sources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public UsefulSourcesContext(
            DbContextOptions<UsefulSourcesContext> opt,
            AuditableEntitySaveChangesInterseptor auditableEntitySaveChangesInterseptor)
            : base(opt)
        {
            _auditableEntitySaveChangesInterseptor = auditableEntitySaveChangesInterseptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterseptor);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfigureForPostgreSQL(builder);
        }
        /*
            For postgresql it's better way to manage database in pgAdmin
            when all properties' and tables' names in lowercase
        */
        private void ConfigureForPostgreSQL(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
                foreach (var property in entityType.GetProperties())
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasColumnName(property.Name.ToLower());

                    if (property.Name == "Id")
                        builder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .UseIdentityAlwaysColumn();
                }
        }
    }
}
