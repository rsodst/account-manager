using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Domain;
using Modulbank.Profiles.Queries;
using Modulbank.Profiles.Specification;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("profile/person-details")]
    [ApiController]
    [Authorize]
    public class PersonDetailsController : AppController
    {
        private readonly IMediator _mediator;

        public PersonDetailsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Get()
        {
            var query = new GetPersonDetailsQuery
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Post(PersonDetailsModel model)
        {
            var command = model.Adapt<CreatePersonDetailsCommand>();
            
            command.UserId = CurrentUserId;
            
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Put(PersonDetailsModel model)
        {
            var command = model.Adapt<UpdatePersonDetailsCommand>();

            command.UserId = CurrentUserId;
            
            return Ok(await _mediator.Send(command));
        }

        // models
        
        public class PersonDetailsModel : IPersonDetails
        {
            [Required] 
            [DataType(DataType.Text)]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            public string MiddleName { get; set; }
        }
    }
}