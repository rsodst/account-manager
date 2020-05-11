using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Accounts.Commands;
using Modulbank.Accounts.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Accounts.RequestHandlers
{
    public class UpdateTransactionCommandhandler : IRequestHandler<UpdateTransactionCommand, Transaction>
    {
        private TransactionsTable _table;

        public UpdateTransactionCommandhandler(IAccountsContext context)
        {
            _table = new TransactionsTable(context);
        }

        public async Task<Transaction> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _table.GetByIdAsync(request.TransactionId);

            transaction.Status = request.Status;
            transaction.ProceedDate = DateTime.UtcNow;

            return await _table.UpdateAsync(transaction);
        }
    }
}