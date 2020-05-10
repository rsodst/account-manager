using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Commands
{
    public class UpdateProfileConfirmationCommand : IRequest<ProfileConfirmation>
    {
        public ProfileConfirmation ProfileConfirmation { get; set; }
    }
}