using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, Account>
    {
        private AccountsTable _table;
        
        public CloseAccountCommandHandler(IAccountsContext context)
        {
            _table = new AccountsTable(context);
        }
        
        public async Task<Account> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _table.GetAsync(request.AccountId, request.UserId);

            if (account == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Account not found");
            }
            
            account.IsDeleted = true;
            account.LastModified = DateTime.UtcNow;

            return await _table.UpdateAsync(account);
        }
    }
}