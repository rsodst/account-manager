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
    public class ProfileConfirmationTable 
    {
        private IProfilesContext _context;

        public ProfileConfirmationTable(IProfilesContext _context)
        {
            this._context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<ProfileConfirmation> CreateAsync(ProfileConfirmation profileConfirmation)
        {
            var command = "INSERT INTO public.\"ProfileConfirmation\" " +
                          " (" +
                          $"\"{nameof(ProfileConfirmation.Id)}\", " +
                          $"\"{nameof(ProfileConfirmation.UserId)}\", " +
                          $"\"{nameof(ProfileConfirmation.CreationDate)}\", " +
                          $"\"{nameof(ProfileConfirmation.LastModified)}\", " +
                          $"\"{nameof(ProfileConfirmation.IsDeleted)}\", " +
                          $"\"{nameof(ProfileConfirmation.Description)}\") " +
                          "VALUES (@Id, @UserId, @CreationDate, @LastModified, @IsDeleted, @Description);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    profileConfirmation.Id,
                    profileConfirmation.UserId,
                    profileConfirmation.CreationDate,
                    profileConfirmation.LastModified,
                    profileConfirmation.IsDeleted,
                    profileConfirmation.Description
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(ProfileConfirmation)} object");

            return profileConfirmation;
        }

        public async Task<ProfileConfirmation> UpdateAsync(ProfileConfirmation profileConfirmation)
        {
            var command = "UPDATE public.\"ProfileConfirmation\" " +
                          "SET" +
                          $"\"{nameof(ProfileConfirmation.IsDeleted)}\" = @IsDeleted, " +
                          $"\"{nameof(ProfileConfirmation.LastModified)}\" = @LastModified" +
                          $" where \"{nameof(ProfileConfirmation.UserId)}\" = @UserId";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                await sqlConnection.ExecuteAsync(command, new
                {
                    profileConfirmation.IsDeleted,
                    profileConfirmation.LastModified,
                    profileConfirmation.UserId
                });
            }

            return profileConfirmation;
        }

        public async Task<ProfileConfirmation> GetAsync(Guid userId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"ProfileConfirmation\" " +
                          $"WHERE \"{nameof(ProfileConfirmation.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<ProfileConfirmation>(command, new {UserId = userId});
            }
        }
    }
}