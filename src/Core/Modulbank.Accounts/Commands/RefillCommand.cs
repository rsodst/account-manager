using System;
using MediatR;

namespace Modulbank.Accounts.Commands
{
    public class RefillCommand : IRequest
    {
        public Guid UserId { get; set; }        
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}