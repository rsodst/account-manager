using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Command
{
    public class GetPersonDetailsQuery : IRequest<PersonDetails>
    {
        public Guid UserId { get; set; }
    }
}