using System;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Commands
{
    public class UpdateTransactionCommand : IRequest<Transaction>
    {
        public Guid TransactionId { get; set; }
        
        public TransactionStatus Status { get; set; }
    }
}