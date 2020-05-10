using System;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Specification;

namespace Modulbank.Accounts.Commands
{
    public class EditAccountCommand : IRequest<Account>, IAccountDetail
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        
        public decimal LimitByOperation { get; set; }
        
        public string Description { get; set; }
        
        public Currency Currency { get; set; }
    }
}