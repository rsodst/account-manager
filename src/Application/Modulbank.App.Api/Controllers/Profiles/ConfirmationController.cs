using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Profiles.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("profile/confirmation")]
    [ApiController]
    [Authorize]
    public class ConfirmationController : AppController
    {
        private readonly IMediator _mediator;

        public ConfirmationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Get()
        {
            var query = new GetProfileConfirmationQuery
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(query));
        }
    }
}