using Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        { 
            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationMaganager>();

            return services;
        }
    }
}
