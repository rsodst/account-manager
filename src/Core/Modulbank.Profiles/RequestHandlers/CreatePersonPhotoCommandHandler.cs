using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Command;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class CreatePersonPhotoCommandHandler : IRequestHandler<CreatePersonPhotoCommand, PersonPhoto>
    {
        private readonly PersonPhotoTable _table;
        
        public CreatePersonPhotoCommandHandler(IProfilesContext context)
        {
            _table = new PersonPhotoTable(context);
        }

        public async Task<PersonPhoto> Handle(CreatePersonPhotoCommand request, CancellationToken cancellationToken)
        {
            var personPhoto = new PersonPhoto
            {
                UserId = request.UserId,
                FileName = request.FileName,
                CreationDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            await _table.CreateAsync(personPhoto);

            return personPhoto;
        }
    }
}