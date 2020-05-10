using System;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;
using Modulbank.Accounts.Specification;

namespace Modulbank.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<Account>, IAccountDetail
    {
        public Guid UserId { get; set; }

        public Currency Currency { get; set; }

        public decimal LimitByOperation { get; set; }
        
        public string Description { get; set; }
    }
}