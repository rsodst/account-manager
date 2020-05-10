using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class UpdatePersonPhotoCommandHandler : IRequestHandler<UpdatePersonPhotoCommand, PersonPhoto>
    {
        private readonly PersonPhotoTable _table;
        
        public UpdatePersonPhotoCommandHandler(IProfilesContext context) 
        {
            _table = new PersonPhotoTable(context);
        }

        public async Task<PersonPhoto> Handle(UpdatePersonPhotoCommand request, CancellationToken cancellationToken)
        {
            var personPhoto = await _table.GetAsync(request.UserId);

            personPhoto.FileName = request.FileName;
            personPhoto.LastModified = DateTime.UtcNow;
            
            return await _table.UpdateAsync(personPhoto);
        }
    }
}