using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Data.Context;
using Modulbank.Users;
using Modulbank.Users.Domain;

namespace Identity.Dapper.Postgres.Tables
{
    internal class UserClaimsTable
    {
        private readonly IUsersContext _context;

        public UserClaimsTable(IUsersContext databaseConnectionFactory)
        {
            _context = databaseConnectionFactory;
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var command = "SELECT * " +
                          "FROM public.\"UserClaims\" " +
                          $"WHERE \"{nameof(UserClaim.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return (
                        await sqlConnection.QueryAsync<UserClaim>(command, new {UserId = user.Id})
                    )
                    .Select(e => new Claim(e.ClaimType, e.ClaimValue))
                    .ToList();
                ;
            }
        }
    }
}