using System;
using MediatR;
using Modulbank.Users.Domain;

namespace Modulbank.Users.Queries
{
    public class GetUserQuery : IRequest<ApplicationUser>
    {
        public Guid UserId { get; set; }
    }
}