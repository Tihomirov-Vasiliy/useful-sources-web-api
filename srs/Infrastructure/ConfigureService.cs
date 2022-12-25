using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Context;
using Infrastructure.Data;
using Application.Common.Interfaces;
using Infrastructure.Services;
using Infrastructure.Interseptors;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<AuditableEntitySaveChangesInterseptor>();

            services.AddScoped<IUsefulSourcesRepo, SqlUsefulSourcesRepo>();

            //Testing API by using mock repo in DI
            //services.AddScoped<IUsefulSourcesRepo, MockUsefulSourcesRepo>();

            services.AddDbContext<UsefulSourcesContext>(opt => opt
                .UseNpgsql(config.GetConnectionString("Db")));

            //add db initializer
            services.AddScoped<UsefulSourcesContextInitializer>();
            //add datetimeservice to container
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICurrentUserService, CurrentUserServiceMock>();

            return services;
        }
    }
}
