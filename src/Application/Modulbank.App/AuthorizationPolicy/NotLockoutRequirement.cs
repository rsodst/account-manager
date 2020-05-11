using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Domain;

namespace Modulbank.App.AuthorizationPolicy
{
    public class NotLockoutRequirement : AuthorizationHandler<NotLockoutRequirement>, IAuthorizationRequirement
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NotLockoutRequirement(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotLockoutRequirement requirement)
        {
            var userId = context.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ApplicationApiException(HttpStatusCode.Unauthorized, "User not authenticated");
            }
            
            var user = await _userManager.FindByIdAsync(userId);

            var isLockout = await _userManager.IsLockedOutAsync(user);

            if (isLockout)
                context.Fail();
            else
                context.Succeed(requirement);
        }
    }
}
