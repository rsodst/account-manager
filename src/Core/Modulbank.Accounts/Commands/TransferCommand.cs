using System;
using MediatR;

namespace Modulbank.Accounts.Commands
{
    public class TransferCommand : IRequest
    {
        public Guid TransactionId { get; set; }
    }
}