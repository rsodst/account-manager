using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Data.Context;
using Modulbank.Users.Models;

namespace Identity.Dapper.Postgres.Tables
{
    internal class UserTokensTable
    {
        private readonly IUsersContext _context;

        public UserTokensTable(IUsersContext databaseConnectionFactory) => _context = databaseConnectionFactory;

        public async Task<IEnumerable<UserToken>> GetTokensAsync(Guid userId)
        {
            var command = "SELECT * " +
                                   "FROM public.\"UserTokens\" " +
                                   $"WHERE \"{nameof(UserToken.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<UserToken>(command, new
                {
                    UserId = userId
                });
            }
        }
    }
}
