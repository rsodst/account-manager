using System;
using MediatR;
using Modulbank.Accounts.Domain;

namespace Modulbank.Accounts.Queries
{
    public class FindAccountQuery : IRequest<Account>
    {
        public long Number;
    }
}