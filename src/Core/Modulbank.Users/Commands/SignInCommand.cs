using MediatR;
using Modulbank.Users.Domain;

namespace Modulbank.Users.Commands
{
    public interface ISignInCommandSpecification
    {
        string Email { get; set; }
        string Password { get; set; }
    }

    public class SignInCommand : IRequest<UserToken>, ISignInCommandSpecification
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}