using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Account>
    {
        private AccountsTable _table;

        public GetAccountQueryHandler(IAccountsContext context)
        {
            _table = new AccountsTable(context);
        }
        
        public async Task<Account> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetAsync(request.AccountId, request.UserId);
        }
    }
}