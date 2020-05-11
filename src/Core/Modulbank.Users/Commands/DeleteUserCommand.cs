using System;
using MediatR;

namespace Modulbank.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}