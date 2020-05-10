using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class UpdateProfileConfirmationCommandHandler : IRequestHandler<UpdateProfileConfirmationCommand, ProfileConfirmation>
    {
        private readonly ProfileConfirmationTable _table;
        
        public UpdateProfileConfirmationCommandHandler(IProfilesContext context) 
        {
            _table = new ProfileConfirmationTable(context);
        }

        public async Task<ProfileConfirmation> Handle(UpdateProfileConfirmationCommand request, CancellationToken cancellationToken)
        {
            request.ProfileConfirmation.LastModified = DateTime.UtcNow;

            return await _table.UpdateAsync(request.ProfileConfirmation);
        }
    }
}