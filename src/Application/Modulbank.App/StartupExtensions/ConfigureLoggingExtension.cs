using Microsoft.Extensions.DependencyInjection;
using Npgsql.Logging;

namespace Modulbank.App.StartupExtensions
{
    public static class ConfigureLoggingExtension
    {
        public static IServiceCollection ConfigureApplicationLogging(this IServiceCollection services)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);

            return services;
        }
    }
}