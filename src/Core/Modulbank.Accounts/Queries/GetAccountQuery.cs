using System;
using MediatR;
using Modulbank.Accounts.Domain;

namespace Modulbank.Accounts.Queries
{
    public class GetAccountQuery : IRequest<Account>
    {
        public Guid UserId;
        
        public Guid AccountId;
    }
}