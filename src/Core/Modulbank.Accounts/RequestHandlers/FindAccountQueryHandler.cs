using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class FindAccountQueryHandler : IRequestHandler<FindAccountQuery, Account>
    {
        private AccountsTable _table;

        public FindAccountQueryHandler(IAccountsContext context)
        {
            _table = new AccountsTable(context);
        }
        
        public async Task<Account> Handle(FindAccountQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetByNumberAsync(request.Number);
        }
    }
}