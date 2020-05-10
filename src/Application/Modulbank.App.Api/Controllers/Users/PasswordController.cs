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
    [Route("user/password")]
    [ApiController]
    [Authorize]
    public class UpdatePasswordController : AppController
    {
        private readonly IMediator _mediator;

        public UpdatePasswordController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> Put(UpdatePasswordModel model)
        {
            var command = model.Adapt<UpdatePasswordCommand>();

            command.UserId = CurrentUserId;

            return Ok(await _mediator.Send(command));
        }

        // models
        
        public class UpdatePasswordModel : IUpdatePasswordCommandSpecification
        {
            [Required]
            [DataType(DataType.Password)]
            public string CurrentPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }
        }
    }
}