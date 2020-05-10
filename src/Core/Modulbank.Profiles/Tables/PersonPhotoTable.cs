using System;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Profiles;
using Modulbank.Profiles.Domain;
using Modulbank.Shared.Exceptions;

namespace Modulbank.Users.Tables
{
    public class PersonPhotoTable : IGeneralTable<PersonPhoto>
    {
        private IProfilesContext _context;

        public PersonPhotoTable(IProfilesContext _context)
        {
            this._context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<PersonPhoto> CreateAsync(PersonPhoto personPhoto)
        {
            var command = "INSERT INTO public.\"PersonPhoto\" " +
                          " (" +
                          $"\"{nameof(PersonPhoto.Id)}\", " +
                          $"\"{nameof(PersonPhoto.UserId)}\") " +
                          $"\"{nameof(PersonPhoto.FileName)}\", " +
                          $"\"{nameof(PersonPhoto.CreationDate)}\", " +
                          $"\"{nameof(PersonPhoto.LastModified)}\", " +
                          "VALUES (@Id, @UserId, @FileName, @CreationDate, @LastModified);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    personPhoto.Id,
                    personPhoto.UserId,
                    personPhoto.FileName,
                    personPhoto.CreationDate,
                    personPhoto.LastModified
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(PersonPhoto)} object");

            return personPhoto;
        }

        public async Task<PersonPhoto> UpdateAsync(PersonPhoto personPhoto)
        {
            var command = "UPDATE public.\"PersonPhoto\" " +
                          "SET" +
                          $"\"{nameof(PersonPhoto.FileName)}\" = @FileName, " +
                          $"\"{nameof(PersonPhoto.LastModified)}\" = @LastModified" +
                          $"where \"{nameof(PersonPhoto.UserId)}\" = @UserId";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                await sqlConnection.ExecuteAsync(command, new
                {
                    personPhoto.FileName,
                    personPhoto.LastModified
                });
            }

            return personPhoto;
        }

        public async Task<PersonPhoto> GetAsync(Guid userId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"PersonPhoto\" " +
                          $"WHERE \"{nameof(PersonPhoto.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<PersonPhoto>(command, new {UserId = userId});
            }
        }
    }
}