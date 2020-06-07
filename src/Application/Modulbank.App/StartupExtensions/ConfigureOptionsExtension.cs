using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulbank.Settings;
using System.Collections.Generic;

namespace Modulbank.App.StartupExtensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<PostgresConnections>(configuration.GetSection(typeof(PostgresConnections).Name));
            services.Configure<JwtOptions>(configuration.GetSection(typeof(JwtOptions).Name));
            services.Configure<FileStorageOptions>(configuration.GetSection(typeof(FileStorageOptions).Name));

            return services;
        }
    }
}