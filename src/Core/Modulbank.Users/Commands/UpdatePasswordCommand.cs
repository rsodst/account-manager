using System;
using MediatR;

namespace Modulbank.Users.Commands
{
    public interface IUpdatePasswordCommandSpecification
    {
        string CurrentPassword { get; set; }
        string NewPassword { get; set; }
    }

    public class UpdatePasswordCommand : IRequest, IUpdatePasswordCommandSpecification
    {
        public Guid UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}