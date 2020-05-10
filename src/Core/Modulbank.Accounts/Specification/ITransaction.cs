using System;
using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Specification
{
    public interface ITransaction
    {
        Guid WriteOffAccount { get; set; }

        Guid DestinationAccount { get; set; }

        TransactionType Type { get; set; }
    }
}