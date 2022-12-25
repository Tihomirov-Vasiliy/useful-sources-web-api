using Services.ApplicationServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISourcesService, SourcesService>();
            services.AddScoped<ITagsService, TagsService>();

            return services;
        }
    }
}
