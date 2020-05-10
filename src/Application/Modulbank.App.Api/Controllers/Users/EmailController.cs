using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Users.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("user/email")]
    [ApiController]
    [Authorize]
    public class UpdateEmailController : AppController
    {
        private readonly IMediator _mediator;

        public UpdateEmailController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> Put(UpdateEmailModel model)
        {
            var command = model.Adapt<UpdateEmailCommand>();

            command.UserId = CurrentUserId;

            return Ok(await _mediator.Send(command));
        }
        
        [HttpPost("confirm")]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> Post(ConfirmEmailModel model)
        {
            var command = model.Adapt<ConfirmEmailCommand>();

            command.UserId = CurrentUserId;

            return Ok(await _mediator.Send(command));
        }

        // models
        
        public class UpdateEmailModel : IUpdateEmailCommandSpecification
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }
        
        public class ConfirmEmailModel : IConfirmEmailCommandSpecification
        {
            [Required]
            [DataType(DataType.Text)]
            public string ConfirmationToken { get; set; }
        }
    }
}