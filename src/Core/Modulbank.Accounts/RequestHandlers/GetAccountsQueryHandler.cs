using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, List<Account>>
    {
        private readonly AccountsTable _table;

        public GetAccountsQueryHandler(IAccountsContext context)
        {
            _table = new AccountsTable(context);
        }

        public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetListAsync(request.UserId);
        }
    }
}