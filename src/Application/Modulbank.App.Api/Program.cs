using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Modulbank.App.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var environment = context.HostingEnvironment;

                        var infrastructureFolder = Path.Combine(environment.ContentRootPath, "..\\..", "Infrastructure");

                        config
                            .AddJsonFile(Path.Combine(infrastructureFolder, $"appsettings.{environment.EnvironmentName}.json"), true)
                            .AddJsonFile("appsettings.json", true);
                    });
                    webBuilder.ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddConsole();
                    });
                });
        }
    }
}