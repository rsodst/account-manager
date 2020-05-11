using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Accounts
{
    [Route("accounts/{accountId}/refill")]
    [ApiController]
    [Authorize("NotLockoutRequirement")]
    [Authorize("ProfileConfirmedRequirement")]
    public class RefillController : AppController
    {
        private IMediator _mediator;

        public RefillController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Post(RefillModel model, Guid accountId)
        {
            var command = model.Adapt<RefillCommand>();

            command.UserId = CurrentUserId;
            command.AccountId = accountId;

            await _mediator.Send(command);
            
            await _mediator.Send(new LogActionCommand
            {
                AccountId = accountId,
                ActionType = ActionType.Refil
            });

            return Ok();
        }
        
        // Models

        public class RefillModel
        {
            [Required]
            [DataType(DataType.Currency)]
            public decimal Amount { get; set; }
        }
    }
}