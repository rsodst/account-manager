using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Users.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("user/signup")]
    [ApiController]
    public class SignUpController : AppController
    {
        private readonly IMediator _mediator;

        public SignUpController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var command = model.Adapt<SignUpCommand>();

            return Ok(await _mediator.Send(command));
        }

        // models
        
        public class SignUpModel : ISignUpCommandSpecification
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}