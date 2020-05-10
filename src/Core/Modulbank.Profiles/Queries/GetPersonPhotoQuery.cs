using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Command
{
    public class GetPersonPhotoQuery : IRequest<PersonPhoto>
    {
        public Guid UserId { get; set; }
    }
}