using System;
using MediatR;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Commands
{
    public class LogActionCommand : IRequest<AccountAction>
    {
        public Guid AccountId { get; set; }
        
        public ActionType ActionType { get; set; }
    }
}