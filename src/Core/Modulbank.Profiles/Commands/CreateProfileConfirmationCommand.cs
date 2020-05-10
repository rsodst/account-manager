using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Command
{
    public class CreateProfileConfirmationCommand : IRequest<ProfileConfirmation>
    {
        public ProfileConfirmation ProfileConfirmation { get; set; }
    }
}