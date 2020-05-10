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
    [Route("user/signin")]
    [ApiController]
    public class SignInController : AppController
    {
        private readonly IMediator _mediator;

        public SignInController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var command = model.Adapt<SignInCommand>();
            
            return Ok(await _mediator.Send(command));
        }

        // models
        
        public class SignInModel : ISignInCommandSpecification
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