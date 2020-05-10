using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Data.Context;
using Modulbank.Users.Domain;

namespace Modulbank.Users.Tables
{
    internal class RoleClaimsTable
    {
        private IUsersContext _context;

        public RoleClaimsTable(IUsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Claim>> GetClaimsAsync(Guid roleId)
        {
             var command = @"SELECT * " +
                                   "FROM public.\"RoleClaims\" " +
                                   $"WHERE \"{nameof(RoleClaim.RoleId)}\" = @RoleId;";

            IEnumerable<RoleClaim> roleClaims = new List<RoleClaim>();

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return (
                    await sqlConnection.QueryAsync<RoleClaim>(command, new { RoleId = roleId })
                )
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
            }
        }
    }
}
