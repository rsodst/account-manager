using MediatR;
using Modulbank.Users.Models;

namespace Modulbank.Users.Commands
{
    public interface ISignUpCommandSpecification
    {
        string Email { get; set; }
        string Password { get; set; }
    }

    public class SignUpCommand : IRequest<UserToken>, ISignUpCommandSpecification
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}