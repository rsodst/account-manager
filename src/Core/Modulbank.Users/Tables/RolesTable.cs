using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Modulbank.Data.Context;
using Modulbank.Users.Domain;

namespace Modulbank.Users.Tables
{
    internal class RolesTable
    {
        private readonly IUsersContext _context;

        public RolesTable(IUsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            var command = "INSERT INTO public.\"Roles\" " +
                          "(" +
                          $"\"{nameof(ApplicationRole.Id)}\"," +
                          $"\"{nameof(ApplicationRole.Name)}\", " +
                          $"\"{nameof(ApplicationRole.NormalizedName)}\", " +
                          $"\"{nameof(ApplicationRole.ConcurrencyStamp)}\") " +
                          "VALUES (@Id, @Name, @NormalizedName, @ConcurrencyStamp);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    role.Id,
                    role.Name,
                    role.NormalizedName,
                    role.ConcurrencyStamp
                });
            }

            return rowsInserted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = string.Empty,
                    Description = $"The role with name {role.Name} could not be inserted."
                });
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role)
        {
            var command = "UPDATE public.\"Roles\" " +
                          $"SET \"{nameof(ApplicationRole.Name)}\" = @Name, NormalizedName = @NormalizedName, ConcurrencyStamp = @ConcurrencyStamp " +
                          "WHERE Id = @Id;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    await sqlConnection.ExecuteAsync(command, new
                    {
                        role.Name,
                        role.NormalizedName,
                        role.ConcurrencyStamp,
                        role.Id
                    }, transaction);

                    if (role.Claims.Any())
                    {
                        var deleteClaimsCommand = "DELETE " +
                                                  "FROM public.\"RoleClaims\" " +
                                                  $"WHERE \"{nameof(RoleClaim.RoleId)}\" = @RoleId;";

                        await sqlConnection.ExecuteAsync(deleteClaimsCommand, new
                        {
                            RoleId = role.Id
                        }, transaction);

                        var insertClaimsCommand = "INSERT INTO public.\"RoleClaims\"" +
                                                  "(" +
                                                  $"\"{nameof(RoleClaim.RoleId)}\"," +
                                                  $"\"{nameof(RoleClaim.ClaimType)}\"," +
                                                  $"\"{nameof(RoleClaim.ClaimValue)}\"" +
                                                  "VALUES (@RoleId, @ClaimType, @ClaimValue);";

                        await sqlConnection.ExecuteAsync(insertClaimsCommand, role.Claims.Select(x => new
                        {
                            RoleId = role.Id,
                            ClaimType = x.Type,
                            ClaimValue = x.Value
                        }), transaction);
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            return IdentityResult.Failed(new IdentityError
                            {
                                Code = nameof(UpdateAsync),
                                Description = $"Role with name {role.Name} could not be updated. Operation could not be rolled back."
                            });
                        }

                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = nameof(UpdateAsync),
                            Description = $"Role with name {role.Name} could not be updated.. Operation was rolled back."
                        });
                    }
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            var command = "DELETE " +
                          "FROM public.\"Roles\" " +
                          $"WHERE \"{nameof(ApplicationRole.Id)}\" = @Id;";

            int rowsDeleted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsDeleted = await sqlConnection.ExecuteAsync(command, new {role.Id});
            }

            return rowsDeleted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = string.Empty,
                    Description = $"The role with name {role.Name} could not be deleted."
                });
        }

        public async Task<ApplicationRole> FindByIdAsync(Guid roleId)
        {
            var command = "SELECT * " +
                          "FROM public.\"Roles\" " +
                          $"WHERE \"{nameof(ApplicationRole.Id)}\" = @Id;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationRole>(command, new
                {
                    Id = roleId
                });
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName)
        {
            var command = "SELECT * " +
                          "FROM public.\"Roles\" " +
                          $"WHERE \"{nameof(ApplicationRole.NormalizedName)}\" = @NormalizedName;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationRole>(command, new
                {
                    NormalizedName = normalizedRoleName
                });
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            const string command = "SELECT * " +
                                   "FROM public.\"Roles\";";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<ApplicationRole>(command);
            }
        }
    }
}