using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class EditAccountCommandHandler : IRequestHandler<EditAccountCommand, Account>
    {
        private readonly AccountsTable _accountsTable;

        public EditAccountCommandHandler(IAccountsContext context)
        {
            _accountsTable = new AccountsTable(context);
        }

        public async Task<Account> Handle(EditAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountsTable.GetAsync(request.AccountId, request.UserId);

            if (account == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Account not found");
            }
            
            request.Adapt(account.AccountDetail);

            account.LastModified = DateTime.UtcNow;
            account.AccountDetail.LastModified = account.LastModified;

            return await _accountsTable.UpdateAsync(account);
        }
    }
}