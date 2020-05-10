using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.FileStorage.Queries;

namespace Modulbank.App.Api.Controllers.Storage
{
    [Route("storage/person-photo")]
    [ApiController]
    [Authorize]
    public class FileStorageController : AppController
    {
        private readonly IMediator _mediator;

        public FileStorageController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("{filename}")]
        public async Task<IActionResult> DownloadPhoto(GetPhotoContentQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}