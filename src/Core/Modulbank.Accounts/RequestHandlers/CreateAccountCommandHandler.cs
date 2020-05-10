using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Settings;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly AccountOptions _accountOptions;
        private readonly AccountsTable _table;

        public CreateAccountCommandHandler(IAccountsContext context, IOptions<AccountOptions> accountOptions)
        {
            _table = new AccountsTable(context);
            _accountOptions = accountOptions.Value ?? throw new ArgumentNullException(nameof(accountOptions));
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                UserId = request.UserId,
                CreationDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddYears(_accountOptions.DefaultLifetimeYear)
            };

            var accountDetail = new AccountDetail
            {
                AccountId = account.Id,
                Description = request.Description,
                LimitByOperation = request.LimitByOperation,
                LastModified = account.LastModified,
                Currency = request.Currency,
                CreationDate = account.CreationDate
            };

            account.AccountDetail = accountDetail;

            return await _table.CreateAsync(account);
        }
    }
}