using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Domain;
using Modulbank.Profiles.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class GetPersonPhotoQueryHandler : IRequestHandler<GetPersonPhotoQuery, PersonPhoto>
    {
        private readonly PersonPhotoTable _table;
        
        public GetPersonPhotoQueryHandler(IProfilesContext context)
        {
            _table = new PersonPhotoTable(context);
        }

        public async Task<PersonPhoto> Handle(GetPersonPhotoQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetAsync(request.UserId);
        }
    }
}