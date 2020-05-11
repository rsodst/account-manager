using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class RefillCommandHandler : IRequestHandler<RefillCommand>
    {
        private AccountsTable _table;

        public RefillCommandHandler(IAccountsContext context)
        {
            _table = new AccountsTable(context);
        }

        public async Task<Unit> Handle(RefillCommand request, CancellationToken cancellationToken)
        {
            var account = await _table.GetAsync(request.AccountId, request.UserId);
            
            if (account == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Account not found");
            }

            account.Balance += request.Amount;
            account.LastModified = DateTime.UtcNow;

            await _table.UpdateAsync(account);
            
            return Unit.Value;
        }
    }
}