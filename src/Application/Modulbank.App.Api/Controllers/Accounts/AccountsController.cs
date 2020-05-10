using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Queries;
using Modulbank.Accounts.Specification;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Accounts
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class AccountsController : AppController
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list/{skip}/{take}")]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Get(int skip = 0, int take = 10)
        {
            var query = new GetAccountsQuery
            {
                UserId = CurrentUserId,
                Skip = skip,
                Take = take
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{accountId}")]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var query = new GetAccountQuery
            {
                UserId = CurrentUserId,
                AccountId = accountId
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Create([FromBody] AccountModel model)
        {
            var command = model.Adapt<CreateAccountCommand>();

            command.UserId = CurrentUserId;

            var account = await _mediator.Send(command);

            await _mediator.Send(new LogActionCommand
            {
                AccountId = account.Id,
                ActionType = ActionType.Create
            });

            return Ok(account);
        }

        [HttpDelete("{accountId}")]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Close(Guid accountId)
        {
            var command = new CloseAccountCommand
            {
                UserId = CurrentUserId,
                AccountId = accountId
            };

            var account = await _mediator.Send(command);

            await _mediator.Send(new LogActionCommand
            {
                AccountId = account.Id,
                ActionType = ActionType.Close
            });

            return Ok(account);
        }

        [HttpPut("{accountId}")]
        [SwaggerOperation(Tags = new[] {"Account"})]
        public async Task<IActionResult> Edit(AccountModel model, Guid accountId)
        {
            var command = model.Adapt<EditAccountCommand>();

            command.UserId = CurrentUserId;
            command.AccountId = accountId;

            var account = await _mediator.Send(command);

            await _mediator.Send(new LogActionCommand
            {
                AccountId = account.Id,
                ActionType = ActionType.Edit
            });

            return Ok(account);
        }

        // models

        public class AccountModel : IAccountDetail
        {
            [DataType(DataType.Currency)] public decimal LimitByOperation { get; set; }

            [DataType(DataType.Text)] public string Description { get; set; }

            [DefaultValue(Currency.Rub)] public Currency Currency { get; set; }
        }
    }
}