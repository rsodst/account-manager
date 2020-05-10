using System;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Domain
{
    public class AccountAction
    {
        public AccountAction()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        
        public Guid AccountId { get; set; }
        
        public ActionType Type { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}