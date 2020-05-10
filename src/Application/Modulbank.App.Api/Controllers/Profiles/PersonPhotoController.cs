using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.FileStorage.Commands;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("profile/person-photo")]
    [ApiController]
    [Authorize]
    public class PersonPhotoController : AppController
    {
        private readonly IMediator _mediator;

        public PersonPhotoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Get()
        {
            var query = new GetPersonPhotoQuery
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Post(UploadPesonPhotoCommand command)
        {
            var fileName = await _mediator.Send(command);

            var createPhotoCommand = new CreatePersonPhotoCommand
            {
                UserId = CurrentUserId,
                FileName = fileName
            };

            return Ok(await _mediator.Send(createPhotoCommand));
        }

        [HttpPut]
        [SwaggerOperation(Tags = new[] {"Profile"})]
        public async Task<IActionResult> Put(UploadPesonPhotoCommand command)
        {
            var fileName = await _mediator.Send(command);

            var updatePhotoCommand = new UpdatePersonPhotoCommand
            {
                UserId = CurrentUserId,
                FileName = fileName
            };

            return Ok(await _mediator.Send(updatePhotoCommand));
        }
    }
}