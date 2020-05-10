using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Modulbank.App.Api.Controllers.Storage
{
    [Route("storage")]
    [ApiController, Authorize]
    public class FileStorageController : AppController
    {
        private IMediator _mediator;

        public FileStorageController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("photos/{filename}")]
        public async Task<IAsyncResult> GetPhoto(string filename)
        {
            
        }
        
    }
}