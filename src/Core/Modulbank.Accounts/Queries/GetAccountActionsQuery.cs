using System;
using System.Collections.Generic;
using MediatR;
using Modulbank.Accounts.Domain;

namespace Modulbank.Accounts.Queries
{
    public class GetAccountActionsQuery : IRequest<List<AccountAction>>
    {
        public Guid AccountId { get; set; }
        
        public Guid UserId { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }
    }
}