using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Domain;
using Modulbank.Profiles.Queries;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class GetPersonDetailsQueryHandler : IRequestHandler<GetPersonDetailsQuery, PersonDetails>
    {
        private readonly PersonDetailsTable _table;
        
        public GetPersonDetailsQueryHandler(IProfilesContext context)
        {
            _table = new PersonDetailsTable(context);
        }

        public async Task<PersonDetails> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _table.GetAsync(request.UserId);
        }
    }
}