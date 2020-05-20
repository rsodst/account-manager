using System;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Accounts
{
    [Route("accounts/{accountId}/transfer")]
    [ApiController]
    [Authorize("NotLockoutRequirement")]
    [Authorize("ProfileConfirmedRequirement")]
    public class TransferController : AppController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransferController> _logger;

        public TransferController(IMediator mediator, ILogger<TransferController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Post(TransferModel model, Guid accountId)
        {
            var findAccountQuery = new FindAccountQuery();

            findAccountQuery.Number = model.DestinationAccountNumber;
            
            var destinationAccount = await _mediator.Send(findAccountQuery);

            if (destinationAccount == null)
            {
                return BadRequest("Destination account not found");
            }
            
            var startTransactionCommand = model.Adapt<StartTransactionCommand>();

            startTransactionCommand.UserId = CurrentUserId;
            startTransactionCommand.WriteOffAccount = accountId;
            startTransactionCommand.Type = TransactionType.Transfer;
            startTransactionCommand.DestinationAccount = destinationAccount.Id;
            
            var transaction = await _mediator.Send(startTransactionCommand);

            try
            {
                await _mediator.Send(new TransferCommand
                {
                    TransactionId = transaction.Id
                });

                var completeTransaction = await _mediator.Send(new UpdateTransactionCommand
                {
                    TransactionId = transaction.Id,
                    Status = TransactionStatus.Completed
                });
                
                await _mediator.Send(new LogActionCommand
                {
                    AccountId = accountId,
                    ActionType = ActionType.Transfer
                });

                return Ok(completeTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "transfer error");

                var failedTransaction = await _mediator.Send(new UpdateTransactionCommand
                {
                    TransactionId = transaction.Id,
                    Status = TransactionStatus.Error
                });

                throw;
            }
        }

        // Models

        public class TransferModel
        {
            public long DestinationAccountNumber { get; set; }

            public decimal Amount { get; set; }

            public Currency Currency { get; set; }
        }
    }
}