using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Modulbank.Profiles.Queries;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Domain;

namespace Modulbank.App.Attributes
{
    public class ProfileConfirmedFilter :AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private IMediator _mediator;

        public ProfileConfirmedFilter(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            var query = new GetProfileConfirmationQuery
            {
                UserId = Guid.Parse(userId)
            };

            var profileConfirmation = await _mediator.Send(query);

            if (profileConfirmation == null ||
                profileConfirmation.IsDeleted)
            {
                throw new ApplicationApiException(HttpStatusCode.Unauthorized, "User must have confirmation");
            }
        }
    }
}