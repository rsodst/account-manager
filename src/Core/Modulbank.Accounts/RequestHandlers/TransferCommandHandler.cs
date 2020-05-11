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
    public class TransferCommandHandler : IRequestHandler<TransferCommand>
    {
        private readonly AccountsTable _accountsTable;
        private readonly TransactionsTable _transactionsTable;

        public TransferCommandHandler(IAccountsContext context)
        {
            _accountsTable = new AccountsTable(context);
            _transactionsTable = new TransactionsTable(context);
        }

        public async Task<Unit> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionsTable.GetByIdAsync(request.TransactionId);
            
            var writeOffAccount = await _accountsTable.GetAsync(transaction.WriteOffAccount, transaction.UserId);

            ValidateWriteOffAccount(writeOffAccount, transaction);
            
            var destinationAccount = await _accountsTable.GetByIdAsync(transaction.DestinationAccount);
            
            ValidateDestinationAccount(destinationAccount, transaction);

            writeOffAccount.Balance -= transaction.Amount;
            writeOffAccount.LastModified = DateTime.UtcNow;
            
            destinationAccount.Balance += transaction.Amount;
            destinationAccount.LastModified = DateTime.UtcNow;

            await _accountsTable.UpdateAsync(writeOffAccount);
            await _accountsTable.UpdateAsync(destinationAccount);
            
            return Unit.Value;
        }

        // private
        
        private void ValidateWriteOffAccount(Account account, Transaction transaction)
        {
            if (account == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "WriteOff Account not found");
            }
            
            if (account.Balance <= 0)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Insufficient funds");
            }
            
            if ((account.Balance - transaction.Amount) < 0)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Insufficient funds");
            }

            
            if (account.AccountDetail.LimitByOperation <= transaction.Amount)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "operation limit exceeded");
            }
        }

        private void ValidateDestinationAccount(Account account, Transaction transaction)
        {
            if (account == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Destination Account not found");
            }
            
            if (account.AccountDetail.Currency != transaction.Currency)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Сurrencies do not match");
            }
        }
    }
}