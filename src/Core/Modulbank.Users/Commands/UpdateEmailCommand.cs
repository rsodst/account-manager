using System;
using MediatR;

namespace Modulbank.Users.Commands
{
    public interface IUpdateEmailCommandSpecification
    {
        string Email { get; set; }
    }

    public class UpdateEmailCommand : IRequest, IUpdateEmailCommandSpecification
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }
    }
}