using System;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Data;
using Modulbank.Profiles;
using Modulbank.Profiles.Domain;
using Modulbank.Shared.Exceptions;

namespace Modulbank.Users.Tables
{
    public class PersonDetailsTable : IGeneralTable<PersonDetails>
    {
        private IProfilesContext _context;

        public PersonDetailsTable(IProfilesContext _context)
        {
            this._context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<PersonDetails> CreateAsync(PersonDetails personDetails)
        {
            var command = "INSERT INTO public.\"PersonDetails\" " +
                          "(" +
                          $"\"{nameof(PersonDetails.Id)}\", " +
                          $"\"{nameof(PersonDetails.UserId)}\", " +
                          $"\"{nameof(PersonDetails.FirstName)}\", " +
                          $"\"{nameof(PersonDetails.LastName)}\", " +
                          $"\"{nameof(PersonDetails.MiddleName)}\", " +
                          $"\"{nameof(PersonDetails.CreationDate)}\", " +
                          $"\"{nameof(PersonDetails.LastModified)}\")" +
                          "VALUES (@Id, @UserId, @FirstName, @LastName, @MiddleName, @CreationDate, @LastModified);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    personDetails.Id,
                    personDetails.UserId,
                    personDetails.FirstName,
                    personDetails.LastName,
                    personDetails.MiddleName,
                    personDetails.CreationDate,
                    personDetails.LastModified
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(personDetails)} object");

            return personDetails;
        }

        public async Task<PersonDetails> UpdateAsync(PersonDetails personDetails)
        {
            var command = "UPDATE public.\"PersonDetails\"" +
                          "SET" +
                          $"\"{nameof(PersonDetails.FirstName)}\" = @FirstName," +
                          $"\"{nameof(PersonDetails.LastName)}\" = @LastName," +
                          $"\"{nameof(PersonDetails.MiddleName)}\" = @MiddleName, " +
                          $"\"{nameof(PersonDetails.LastModified)}\" = @LastModified" +
                          $" WHERE \"{nameof(PersonDetails.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                await sqlConnection.ExecuteAsync(command, new
                {
                    personDetails.UserId,
                    personDetails.FirstName,
                    personDetails.LastName,
                    personDetails.MiddleName,
                    personDetails.LastModified
                });
            }

            return personDetails;
        }

        public async Task<PersonDetails> GetAsync(Guid userId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"PersonDetails\" " +
                          $"WHERE \"{nameof(PersonDetails.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<PersonDetails>(command, new {UserId = userId});
            }
        }
    }
}