using System;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Specification;

namespace Modulbank.Accounts.Domain
{
    public class Transaction : ITransaction
    {
        public Transaction()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public Decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
        
        public Guid WriteOffAccount { get; set; }
        
        public Guid DestinationAccount { get; set; }

        public TransactionType Type { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime ProceedDate { get; set; }
        
        public TransactionStatus Status { get; set; }
    }
}