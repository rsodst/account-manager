using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulbank.Data.Context;
using Modulbank.Settings;
using Modulbank.Users.Messages;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace Modulbank.App.StartupExtensions
{
    public static class RebusExtensions
    {
        public static IServiceCollection RegisterRebus(this IServiceCollection services, IConfiguration configuration)
        {
            var connections  = configuration.GetSection(typeof(PostgresConnections).Name).Get<PostgresConnections>();

            var connectionOptions = connections.connectionOptions.Single(p => p.Context == typeof(BusContext).Name);

            var connectionString = $"server={connectionOptions.Server};" +
                                   $"userId={connectionOptions.UserId};" +
                                   $"password={connectionOptions.Password};" +
                                   $"Pooling={connectionOptions.Pooling};" +
                                   $"Database=modulbank-bus;";

            services.AddRebus(configurer => configurer
                .Logging(options => options.Use(new ConsoleLoggerFactory(true)))
                .Transport(options => options.UsePostgreSql(connectionString, "messages", "Messages"))
                .Routing(options => 
                    options.TypeBased()
                        .Map<EmailConfirmedMessage>("Messages")
                        .Map<EmailUpdatedMessage>("Messages")
                    )
                .Options(optionsConfigurer =>
                {
                    optionsConfigurer.SetNumberOfWorkers(1);
                    optionsConfigurer.SetMaxParallelism(1);
                    optionsConfigurer.SimpleRetryStrategy("error", 1, true);
                })
            );
            
            

            return services;
        }
    }
}