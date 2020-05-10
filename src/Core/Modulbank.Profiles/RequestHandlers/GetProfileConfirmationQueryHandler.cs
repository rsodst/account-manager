using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Command;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class GetProfileConfirmationQueryHandler : IRequestHandler<GetProfileConfirmationQuery, ProfileConfirmation>
    {
        private readonly ProfileConfirmationTable _table;
        
        public GetProfileConfirmationQueryHandler(IProfilesContext context)
        {
            _table = new ProfileConfirmationTable(context);
        }

        public async Task<ProfileConfirmation> Handle(GetProfileConfirmationQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetAsync(request.UserId);
        }
    }
}