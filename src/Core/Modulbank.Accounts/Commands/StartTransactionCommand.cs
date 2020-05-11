using System;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Commands
{
    public class StartTransactionCommand : IRequest<Transaction>
    {
        public Guid UserId { get; set; }
        
        public Guid WriteOffAccount { get; set; }
        
        public Guid DestinationAccount { get; set; }
        
        public decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
        
        public TransactionType Type { get; set; }
    }
}