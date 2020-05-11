using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulbank.App.AuthorizationPolicy;
using Modulbank.App.Middleware;
using Modulbank.App.StartupExtensions;
using Modulbank.Profiles;
using Modulbank.Users;
using Modulbank.Users.Domain;
using Modulbank.Users.Stores;
using Modulbank.Users.Tables;

namespace Modulbank.App.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApplicationOptions(Configuration);

            //services.ConfigureApplicationLogging();

            services.RegisterDbContexts(Configuration);

            services.RegisterApplicationServices(Configuration);

            services.ConfigureIdentity();

            services.AddControllers();

            services.ConfigureSwagger();

            services.ConfigureJwt(Configuration);

            services.RegisterRebus(Configuration);

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthorization(options =>
            {
                var serviceProvider = services.BuildServiceProvider();

                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var profileContext = serviceProvider.GetService<IProfilesContext>();
                
                options.AddPolicy(typeof(NotLockoutRequirement).Name, builder =>
                {
                    builder.Requirements.Add(new NotLockoutRequirement(userManager));
                });
                
                options.AddPolicy(typeof(ProfileConfirmedRequirement).Name, builder =>
                {
                    builder.Requirements.Add(new ProfileConfirmedRequirement(profileContext));
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.EnsureCreateDbContexts();

            if (Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(configuration =>
            {
                configuration.AllowAnyMethod();
                configuration.AllowAnyHeader();
                configuration.AllowAnyOrigin();
            });

            app.UseSwagger();

            app.UseSwaggerUI(configure =>
            {
                configure.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                configure.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}