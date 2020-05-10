using System;
using System.Transactions;
using MediatR;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Specification;

namespace Modulbank.Accounts.Commands
{
    public class CreateTransactionCommand : IRequest<Transaction>, ITransaction
    {
        public Guid UserId { get; set; }

        public Guid WriteOffAccount { get; set; }

        public Guid DestinationAccount { get; set; }

        public TransactionType Type { get; set; }
    }
}