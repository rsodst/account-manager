using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Modulbank.Data.Context;
using Modulbank.Users;
using Modulbank.Users.Domain;

namespace Identity.Dapper.Postgres.Tables
{
    internal class UsersTable
    {
        private readonly IUsersContext _context;

        public UsersTable(IUsersContext databaseConnectionFactory)
        {
            _context = databaseConnectionFactory;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var command = "INSERT INTO public.\"Users\" " +
                          " (" +
                          $"\"{nameof(ApplicationUser.Id)}\", " +
                          $"\"{nameof(ApplicationUser.UserName)}\", " +
                          $"\"{nameof(ApplicationUser.NormalizedUserName)}\", " +
                          $"\"{nameof(ApplicationUser.Email)}\", " +
                          $"\"{nameof(ApplicationUser.NormalizedEmail)}\", " +
                          $"\"{nameof(ApplicationUser.EmailConfirmed)}\", " +
                          $"\"{nameof(ApplicationUser.PasswordHash)}\", " +
                          $"\"{nameof(ApplicationUser.SecurityStamp)}\", " +
                          $"\"{nameof(ApplicationUser.ConcurrencyStamp)}\", " +
                          $"\"{nameof(ApplicationUser.PhoneNumber)}\", " +
                          $"\"{nameof(ApplicationUser.PhoneNumberConfirmed)}\", " +
                          $"\"{nameof(ApplicationUser.TwoFactorEnabled)}\", " +
                          $"\"{nameof(ApplicationUser.LockoutEnd)}\", " +
                          $"\"{nameof(ApplicationUser.LockoutEnabled)}\", " +
                          $"\"{nameof(ApplicationUser.AccessFailedCount)}\") " +
                          "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, " +
                          "@PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    user.Id,
                    user.UserName,
                    user.NormalizedUserName,
                    user.Email,
                    user.NormalizedEmail,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.ConcurrencyStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEnd,
                    user.LockoutEnabled,
                    user.AccessFailedCount
                });
            }

            return rowsInserted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = nameof(CreateAsync),
                    Description = $"User with Email {user.Email} could not be inserted."
                });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            var command = "delete " +
                          "FROM public.\"Users\" " +
                          $"WHERE \"{nameof(ApplicationUser.Id)}\" = @Id;";

            int rowsDeleted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsDeleted = await sqlConnection.ExecuteAsync(command, new
                {
                    user.Id
                });
            }

            return rowsDeleted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = nameof(DeleteAsync),
                    Description = $"User with Email {user.Email} could not be deleted."
                });
        }

        public async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            var command = "SELECT * " +
                          "FROM public.\"Users\" " +
                          $"WHERE \"{nameof(ApplicationUser.Id)}\" = @userId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(command, new
                {
                    userId
                });
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName)
        {
            var command = "SELECT * " +
                          "FROM public.\"Users\" " +
                          $"WHERE \"{nameof(ApplicationUser.NormalizedUserName)}\" = @normalizedUserName;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(command, new
                {
                    normalizedUserName
                });
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedUserName)
        {
            var command = "SELECT * " +
                          "FROM public.\"Users\" " +
                          $"WHERE \"{nameof(ApplicationUser.NormalizedUserName)}\" = @normalizedUserName;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(command, new
                {
                    normalizedUserName
                });
            }
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            var updateUserCommand =
                "UPDATE public.\"Users\"" +
                "SET " +
                $"\"{nameof(ApplicationUser.UserName)}\" = @UserName, " +
                $"\"{nameof(ApplicationUser.NormalizedUserName)}\" = @NormalizedUserName, " +
                $"\"{nameof(ApplicationUser.Email)}\" = @Email, " +
                $"\"{nameof(ApplicationUser.NormalizedEmail)}\" = @NormalizedEmail, " +
                $"\"{nameof(ApplicationUser.EmailConfirmed)}\" = @EmailConfirmed, " +
                $"\"{nameof(ApplicationUser.PasswordHash)}\" = @PasswordHash, " +
                $"\"{nameof(ApplicationUser.SecurityStamp)}\" = @SecurityStamp, " +
                $"\"{nameof(ApplicationUser.ConcurrencyStamp)}\" = @ConcurrencyStamp, " +
                $"\"{nameof(ApplicationUser.PhoneNumber)}\" = @PhoneNumber, " +
                $"\"{nameof(ApplicationUser.PhoneNumberConfirmed)}\" = @PhoneNumberConfirmed, " +
                $"\"{nameof(ApplicationUser.TwoFactorEnabled)}\" = @TwoFactorEnabled, " +
                $"\"{nameof(ApplicationUser.LockoutEnd)}\" = @LockoutEnd, " +
                $"\"{nameof(ApplicationUser.LockoutEnabled)}\" = @LockoutEnabled, " +
                $"\"{nameof(ApplicationUser.AccessFailedCount)}\" = @AccessFailedCount " +
                $"WHERE \"{nameof(ApplicationUser.Id)}\" = @Id;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    await sqlConnection.ExecuteAsync(updateUserCommand, new
                    {
                        user.UserName,
                        user.NormalizedUserName,
                        user.Email,
                        user.NormalizedEmail,
                        user.EmailConfirmed,
                        user.PasswordHash,
                        user.SecurityStamp,
                        user.ConcurrencyStamp,
                        user.PhoneNumber,
                        user.PhoneNumberConfirmed,
                        user.TwoFactorEnabled,
                        user.LockoutEnd,
                        user.LockoutEnabled,
                        user.AccessFailedCount,
                        user.Id
                    }, transaction);

                    if (user.Claims?.Count() > 0)
                    {
                        var deleteClaimsCommand = "DELETE " +
                                                  "FROM public.\"UserClaims\" " +
                                                  $"WHERE \"{nameof(UserClaim.UserId)}\" = @UserId;";

                        await sqlConnection.ExecuteAsync(deleteClaimsCommand, new
                        {
                            UserId = user.Id
                        }, transaction);

                        var insertClaimsCommand =
                            "INSERT INTO public.\"UserClaims\"" +
                            $"(\"{nameof(UserClaim.UserId)}\"," +
                            $"\"{nameof(UserClaim.ClaimType)}\", " +
                            $"\"{nameof(UserClaim.ClaimValue)}\") " +
                            "VALUES (@UserId, @ClaimType, @ClaimValue);";

                        await sqlConnection.ExecuteAsync(insertClaimsCommand, user.Claims.Select(x => new
                        {
                            UserId = user.Id,
                            ClaimType = x.Type,
                            ClaimValue = x.Value
                        }), transaction);
                    }

                    if (user.Logins?.Count() > 0)
                    {
                        var deleteLoginsCommand = "DELETE " +
                                                  "FROM public.\"UserLogins\" " +
                                                  $"WHERE \"{nameof(UserLogin.UserId)}\" = @UserId;";

                        await sqlConnection.ExecuteAsync(deleteLoginsCommand, new
                        {
                            UserId = user.Id
                        }, transaction);

                        var insertLoginsCommand =
                            "INSERT INTO public.\"UserLogins\"" +
                            "(" +
                            $"\"{nameof(UserLogin.LoginProvider)}\", " +
                            $"\"{nameof(UserLogin.ProviderKey)}\", " +
                            $"\"{nameof(UserLogin.ProviderDisplayName)}\", " +
                            $"\"{nameof(UserLogin.UserId)}\") " +
                            "VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId);";

                        await sqlConnection.ExecuteAsync(insertLoginsCommand, user.Logins.Select(x => new
                        {
                            x.LoginProvider,
                            x.ProviderKey,
                            x.ProviderDisplayName,
                            UserId = user.Id
                        }), transaction);
                    }

                    if (user.Roles?.Count() > 0)
                    {
                        var deleteRolesCommand = "DELETE " +
                                                 "FROM public.\"UserRoles\" " +
                                                 $"WHERE \"{nameof(UserRole.UserId)}\" = @UserId;";

                        await sqlConnection.ExecuteAsync(deleteRolesCommand, new
                        {
                            UserId = user.Id
                        }, transaction);

                        var insertRolesCommand = "INSERT INTO public.\"UserRoles\"" +
                                                 "(" +
                                                 $"\"{nameof(UserRole.UserId)}\", " +
                                                 $"\"{nameof(UserRole.RoleId)}\") " +
                                                 "VALUES (@UserId, @RoleId);";

                        await sqlConnection.ExecuteAsync(insertRolesCommand, user.Roles.Select(x => new
                        {
                            UserId = user.Id,
                            x.RoleId
                        }), transaction);
                    }

                    if (user.Tokens?.Count() > 0)
                    {
                        var deleteTokensCommand = "DELETE " +
                                                  "FROM public.\"UserTokens\" " +
                                                  $"WHERE \"{nameof(UserToken.UserId)}\" = @UserId;";

                        await sqlConnection.ExecuteAsync(deleteTokensCommand, new
                        {
                            UserId = user.Id
                        }, transaction);

                        var insertTokensCommand =
                            "INSERT INTO public.\"UserTokens\"" +
                            "(" +
                            $"\"{nameof(UserToken.UserId)}\", " +
                            $"\"{nameof(UserToken.LoginProvider)}\", " +
                            $"\"{nameof(UserToken.Name)}\", " +
                            $"\"{nameof(UserToken.Value)}\") " +
                            "VALUES (@UserId, @LoginProvider, @Name, @Value);";

                        await sqlConnection.ExecuteAsync(insertTokensCommand, user.Tokens.Select(x => new
                        {
                            x.UserId,
                            x.LoginProvider,
                            x.Name,
                            x.Value
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
                                Description =
                                    $"User with Email {user.Email} could not be updated. Operation could not be rolled back."
                            });
                        }

                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = nameof(UpdateAsync),
                            Description =
                                $"User with Email {user.Email} could not be updated. Operation was rolled back."
                        });
                    }
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            const string command = "SELECT * " +
                                   "FROM public.\"Users\" AS u " +
                                   "INNER JOIN public.\"UserRoles\" AS ur ON u.Id = ur.UserId " +
                                   "INNER JOIN public.\"Roles\" AS r ON ur.RoleId = r.Id " +
                                   "WHERE r.name = @RoleName;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return (await sqlConnection.QueryAsync<ApplicationUser>(command, new
                {
                    RoleName = roleName
                })).ToList();
            }
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim)
        {
            const string command = "SELECT * " +
                                   "FROM public.\"Users\" AS u " +
                                   "INNER JOIN public.\"UserClaims\" AS uc ON u.Id = uc.UserId " +
                                   "WHERE uc.ClaimType = @ClaimType AND uc.ClaimValue = @ClaimValue;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return (await sqlConnection.QueryAsync<ApplicationUser>(command, new
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                })).ToList();
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            const string command = "SELECT * " +
                                   "FROM public.\"Users\";";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<ApplicationUser>(command);
            }
        }
    }
}