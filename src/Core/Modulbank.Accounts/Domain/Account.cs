using System;

namespace Modulbank.Accounts.Domain
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        
        public string Number { get; set; }
        
        public Guid UserId { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public decimal Balance { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime LastModified { get; set; }
        
        public DateTime ExpiredDate { get; set; }
        
        // FK
        
        public AccountDetail AccountDetail { get; set; }
    }
}