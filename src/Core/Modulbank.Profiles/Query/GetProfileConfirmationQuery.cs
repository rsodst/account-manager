using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Command
{
    public class GetProfileConfirmationQuery : IRequest<ProfileConfirmation>
    {
        public Guid UserId { get; set; }
    }
}