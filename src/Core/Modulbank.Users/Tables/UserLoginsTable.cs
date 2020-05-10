using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Modulbank.Data.Context;
using Modulbank.Users;
using Modulbank.Users.Domain;

namespace Identity.Dapper.Postgres.Tables
{
    internal class UserLoginsTable
    {
        private readonly IUsersContext _context;

        public UserLoginsTable(IUsersContext databaseConnectionFactory)
        {
            _context = databaseConnectionFactory;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            var command = "SELECT * " +
                          "FROM public.\"UserLogins\" " +
                          $"WHERE \"{nameof(UserLogin.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return (
                        await sqlConnection.QueryAsync<UserLogin>(command, new {UserId = user.Id})
                    )
                    .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
                    .ToList();
                ;
            }
        }

        public async Task<ApplicationUser> FindByLoginAsync(string LoginProvider, string ProviderKey)
        {
            string[] command =
            {
                "SELECT UserId " +
                "FROM public.\"UserLogins\" " +
                $"WHERE \"{nameof(UserLogin.LoginProvider)}\" = @LoginProvider AND ProviderKey = @ProviderKey;"
            };

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                var userId = await sqlConnection.QuerySingleOrDefaultAsync<Guid?>(command[0], new
                {
                    LoginProvider, ProviderKey
                });

                if (userId == null) return null;

                command[0] = "SELECT * " +
                             "FROM public.\"Users\" " +
                             $"WHERE \"{nameof(ApplicationUser.Id)}\" = @Id;";

                return await sqlConnection.QuerySingleAsync<ApplicationUser>(command[0], new {Id = userId});
            }
        }
    }
}