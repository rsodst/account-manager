using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class LogActionCommandHandler : IRequestHandler<LogActionCommand, AccountAction>
    {
        private readonly AccountActionsTable _table;

        public LogActionCommandHandler(IAccountsContext context)
        {
            _table = new AccountActionsTable(context);
        }

        public async Task<AccountAction> Handle(LogActionCommand request, CancellationToken cancellationToken)
        {
            var action = new AccountAction
            {
                AccountId = request.AccountId,
                CreationDate = DateTime.UtcNow,
                Type = request.ActionType
            };

            return await _table.CreateAsync(action);
        }
    }
}