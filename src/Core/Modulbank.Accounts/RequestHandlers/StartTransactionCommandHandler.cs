using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class StartTransactionCommandHandler : IRequestHandler<StartTransactionCommand, Transaction>
    {
        private TransactionsTable _table;

        public StartTransactionCommandHandler(IAccountsContext context)
        {
            _table = new TransactionsTable(context);
        }

        public async Task<Transaction> Handle(StartTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = request.Adapt<Transaction>();
            
            transaction.CreationDate = DateTime.UtcNow;
            transaction.Status = TransactionStatus.Wait;

            return await _table.CreateAsync(transaction);
        }
    }
}