using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulbank.FileStorage.Services;
using Modulbank.Users.Domain;
using Modulbank.Users.Services;
using Modulbank.Users.Stores;

namespace Modulbank.App.StartupExtensions
{
    public static class RegisterServiceExtension
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}