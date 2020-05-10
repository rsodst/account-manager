using System;
using MediatR;
using Modulbank.Profiles.Domain;

namespace Modulbank.Profiles.Command
{
    public class CreatePersonPhotoCommand : IRequest<PersonPhoto>
    {
        public Guid UserId { get; set; }
        
        public string FileName { get; set; }
    }
}