using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Modulbank.Accounts;
using Modulbank.Accounts.Specification;
using Modulbank.Profiles;
using Modulbank.Users.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.App.AuthorizationPolicy
{
    public class ProfileConfirmedRequirement : AuthorizationHandler<ProfileConfirmedRequirement>, IAuthorizationRequirement
    {
        private readonly ProfileConfirmationTable _table;

        public ProfileConfirmedRequirement(IProfilesContext context)
        {
            _table = new ProfileConfirmationTable(context);
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileConfirmedRequirement requirement)
        {
            var userId = context.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            var profileConfirmation = await _table.GetAsync(Guid.Parse(userId));

            if (profileConfirmation == null ||
                profileConfirmation.IsDeleted)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
        }
    }
}
