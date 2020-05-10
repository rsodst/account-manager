using System;
using MediatR;
using Modulbank.Accounts.Domain;

namespace Modulbank.Accounts.Commands
{
    public class CloseAccountCommand : IRequest<Account>
    {
        public Guid UserId { get; set; }
        
        public Guid AccountId { get; set; }
    }
}