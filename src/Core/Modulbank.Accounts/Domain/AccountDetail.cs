using System;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Domain
{
    public class AccountDetail
    {
        public Guid AccountId { get; set; }

        public decimal LimitByOperation { get; set; }
        
        public string Description { get; set; }
        

        public Currency Currency { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}