using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Modulbank.Profiles.Command;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class CreatePersonDetailsCommandHandler : IRequestHandler<CreatePersonDetailsCommand, PersonDetails>
    {
        private readonly PersonDetailsTable _table;
        
        public CreatePersonDetailsCommandHandler(IProfilesContext context)
        {
            _table = new PersonDetailsTable(context);
        }

        public async Task<PersonDetails> Handle(CreatePersonDetailsCommand request, CancellationToken cancellationToken)
        {
            var personDetails = request.Adapt<PersonDetails>();
            
            personDetails.CreationDate = DateTime.UtcNow;
            personDetails.LastModified = DateTime.UtcNow;
            
            await _table.CreateAsync(personDetails);
            
            return personDetails;
        }
    }
}