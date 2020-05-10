using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulbank.Settings;

namespace Modulbank.App.StartupExtensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PostgresConnectionOptions>(configuration.GetSection(typeof(PostgresConnectionOptions).Name));
            services.Configure<JwtOptions>(configuration.GetSection(typeof(JwtOptions).Name));
            services.Configure<FileStorageOptions>(configuration.GetSection(typeof(FileStorageOptions).Name));

            return services;
        }
    }
}