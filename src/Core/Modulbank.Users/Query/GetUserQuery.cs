using System;
using MediatR;
using Modulbank.Users.Models;

namespace Modulbank.Users.Query
{
    public class GetUserQuery : IRequest<ApplicationUser>
    {
        public Guid UserId { get; set; }
    }
}