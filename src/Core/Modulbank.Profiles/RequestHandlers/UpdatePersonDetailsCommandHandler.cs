using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Modulbank.Profiles.Commands;
using Modulbank.Profiles.Domain;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class UpdatePersonDetailsCommandHandler : 
        IRequestHandler<UpdatePersonDetailsCommand, PersonDetails>
    {
        private readonly PersonDetailsTable _table;
        
        public UpdatePersonDetailsCommandHandler(IProfilesContext context) 
        {
            _table = new PersonDetailsTable(context);
        }

        public async Task<PersonDetails> Handle(UpdatePersonDetailsCommand request, CancellationToken cancellationToken)
        {
            var personDetails = await _table.GetAsync(request.UserId);

            if (personDetails == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "Person details not found");
            }

            request.Adapt(personDetails);
            
            personDetails.LastModified = DateTime.UtcNow;;

            return await _table.UpdateAsync(personDetails);
        }
    }
}