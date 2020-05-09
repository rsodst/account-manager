using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Data.Context;
using Modulbank.Users.Models;

namespace Identity.Dapper.Postgres.Tables
{
    internal class UserRolesTable
    {
        private readonly IUsersContext _context;

        public UserRolesTable(IUsersContext databaseConnectionFactory) => _context = databaseConnectionFactory;

        public async Task<IEnumerable<UserRole>> GetRolesAsync(ApplicationUser user)
        {
            const string command = "SELECT r.Id AS RoleId, r.name AS roleName " +
                                   "FROM public.\"Roles\" AS r " +
                                   "INNER JOIN public.\"UserRoles\" AS ur ON ur.RoleId = r.Id " +
                                   "WHERE ur.UserId = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<UserRole>(command, new
                {
                    UserId = user.Id
                });
            }
        }
    }
}
