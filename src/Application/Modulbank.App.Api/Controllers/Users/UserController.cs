using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Users.Commands;
using Modulbank.Users.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("user")]
    [ApiController]
    [Authorize("NotLockoutRequirement")]
    public class UserController : AppController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] {"User"})]
        public async Task<IActionResult> Get()
        {
            var query = new GetUserQuery
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] {"User"})]
        public async Task<IActionResult> Delete()
        {
            var command = new DeleteUserCommand
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(command));
        }
    }
}