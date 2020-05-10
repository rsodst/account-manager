using System;
using System.Collections.Generic;
using MediatR;
using Modulbank.Accounts.Domain;

namespace Modulbank.Accounts.Queries
{
    public class GetAccountsQuery : IRequest<List<Account>>
    {
        public Guid UserId { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }
    }
}