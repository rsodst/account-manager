using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Queries
{
    public class GetPersonPhotoQuery : IRequest<PersonPhoto>
    {
        public Guid UserId { get; set; }
    }
}