﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Users.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Users
{
    [Route("user")]
    public class UserController : AppController
    {
        private IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [SwaggerOperation(Tags = new []{ "User"})]
        public async Task<IActionResult> Get()
        {
            var query = new GetUserQuery
            {
                UserId = CurrentUserId
            };

            return Ok(await _mediator.Send(query));
        }
    }
}