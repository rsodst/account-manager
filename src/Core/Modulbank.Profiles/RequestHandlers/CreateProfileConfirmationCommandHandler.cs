using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Data.Context;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class CreateProfileConfirmationCommandHandler : IRequestHandler<CreateProfileConfirmationCommand, ProfileConfirmation>
    {
        private readonly ProfileConfirmationTable _table;
        
        public CreateProfileConfirmationCommandHandler(IProfilesContext context) 
        {
            _table = new ProfileConfirmationTable(context);
        }

        public async Task<ProfileConfirmation> Handle(CreateProfileConfirmationCommand request, CancellationToken cancellationToken)
        {
            request.ProfileConfirmation.CreationDate = DateTime.UtcNow;
            request.ProfileConfirmation.LastModified = DateTime.UtcNow;

            await _table.CreateAsync(request.ProfileConfirmation);

            return request.ProfileConfirmation;
        }
    }
}