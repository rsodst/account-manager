using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulbank.Settings;

namespace Modulbank.App.StartupExtensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.Configure<PostgresConnectionOptions>(configuration.GetSection(typeof(PostgresConnectionOptions).Name));
                services.Configure<JwtOptions>(configuration.GetSection(typeof(JwtOptions).Name));
                services.Configure<FileStorageOptions>(configuration.GetSection(typeof(FileStorageOptions).Name));
            }
            else
            {
                services.Configure<PostgresConnectionOptions>(options =>
                {
                    options.UserId = "trvosrhgujsbyy";
                    options.Server = "postgres://trvosrhgujsbyy:6ef938c980d95d41d04560ee50787f1e004417eb12c5216a191d25be195d2b16@ec2-34-194-198-176.compute-1.amazonaws.com:5432";
                    options.Password = "6ef938c980d95d41d04560ee50787f1e004417eb12c5216a191d25be195d2b16";
                    options.Pooling = false;
                });

                services.Configure<JwtOptions>(options =>
                {
                    options.Issuer = "account-manager-api";
                    options.Subject = "account-manager-api-request";
                    options.Audience = "account-manager-api-client";
                    options.Secretkey = "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";
                    options.TokenLifeTimeInMinutes = 60 * 24;
                });
            }

            return services;
        }
    }
}