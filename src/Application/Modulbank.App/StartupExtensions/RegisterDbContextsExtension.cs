using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulbank.Data.Context;
using Modulbank.Settings;
using Rebus.Bus;

namespace Modulbank.App.StartupExtensions
{
    public static class RegisterDbContextsExtension
    {
        public static IServiceCollection RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUsersContext, UsersContext>();

            services.AddSingleton<IBusContext, BusContext>();

            return services;
        }

        public static IApplicationBuilder EnsureCreateDbContexts(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IUsersContext>();
            app.ApplicationServices.GetService<IBusContext>();

            return app;
        }
    }
}