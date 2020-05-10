using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulbank.Accounts;
using Modulbank.Data.Context;
using Modulbank.Profiles;
using Modulbank.Settings;
using Modulbank.Users;
using Rebus.Bus;

namespace Modulbank.App.StartupExtensions
{
    public static class RegisterDbContextsExtension
    {
        public static IServiceCollection RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUsersContext, UsersContext>();
            services.AddSingleton<IBusContext, BusContext>();
            services.AddSingleton<IProfilesContext, ProfilesContext>();
            services.AddSingleton<IAccountsContext, AccountsContext>();

            return services;
        }

        public static IApplicationBuilder EnsureCreateDbContexts(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IUsersContext>();
            app.ApplicationServices.GetService<IBusContext>();
            app.ApplicationServices.GetService<IProfilesContext>();
            app.ApplicationServices.GetService<IAccountsContext>();

            return app;
        }
    }
}