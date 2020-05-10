using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class GetAccountActionsQueryHandler : IRequestHandler<GetAccountActionsQuery, List<AccountAction>>
    {
        private readonly AccountActionsTable _table;

        public GetAccountActionsQueryHandler(IAccountsContext context)
        {
            _table = new AccountActionsTable(context);
        }
        
        public async Task<List<AccountAction>> Handle(GetAccountActionsQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetListAsync(request.AccountId, request.UserId);
        }
    }
}