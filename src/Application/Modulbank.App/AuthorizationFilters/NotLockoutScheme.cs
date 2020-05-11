using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Domain;

namespace Modulbank.App.Attributes
{
    public class NotLockoutRequirement : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NotLockoutRequirement(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);

            var isLockout = await _userManager.IsLockedOutAsync(user);

            if (isLockout)
            {
                throw new ApplicationApiException(HttpStatusCode.Unauthorized, "User must not lockout");
            }
        }
    }
}