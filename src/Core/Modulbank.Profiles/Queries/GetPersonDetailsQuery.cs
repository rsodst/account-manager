using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Queries
{
    public class GetPersonDetailsQuery : IRequest<PersonDetails>
    {
        public Guid UserId { get; set; }
    }
}