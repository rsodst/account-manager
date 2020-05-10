using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Commands
{
    public class CreateProfileConfirmationCommand : IRequest<ProfileConfirmation>
    {
        public ProfileConfirmation ProfileConfirmation { get; set; }
    }
}