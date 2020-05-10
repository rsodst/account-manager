using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Accounts;
using Modulbank.Accounts.Domain;
using Modulbank.Accounts.Queries;
using Modulbank.Data;
using Modulbank.Shared.Exceptions;

namespace Modulbank.Users.Tables
{
    public class AccountsTable 
    {
        private readonly IAccountsContext _context;

        public AccountsTable(IAccountsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<Account> CreateAsync(Account entity)
        {
            var createAccountCommand = "INSERT INTO public.\"Accounts\" " +
                 "(" +
                 $"\"{nameof(Account.Id)}\", " +
                 $"\"{nameof(Account.UserId)}\", " +
                 $"\"{nameof(Account.Balance)}\", " +
                 $"\"{nameof(Account.IsDeleted)}\", " +
                 $"\"{nameof(Account.CreationDate)}\", " +
                 $"\"{nameof(Account.LastModified)}\"," +
                 $"\"{nameof(Account.ExpiredDate)}\")" +
                 " VALUES (@Id, @UserId, @Balance, @IsDeleted, @CreationDate, @LastModified, @ExpiredDate);";

            var createAccountDetailCommand = "INSERT INTO public.\"AccountDetails\" " +
                 "(" +
                 $"\"{nameof(AccountDetail.AccountId)}\", " +
                 $"\"{nameof(AccountDetail.Description)}\", " +
                 $"\"{nameof(AccountDetail.LimitByOperation)}\", " +
                 $"\"{nameof(AccountDetail.CreationDate)}\", " +
                 $"\"{nameof(AccountDetail.Currency)}\", " +
                 $"\"{nameof(AccountDetail.LastModified)}\")" +
                 " VALUES (@AccountId, @Description, @LimitByOperation, @CreationDate, @Currency, @LastModified);";
            
            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    await sqlConnection.ExecuteAsync(createAccountCommand, new
                    {
                        entity.Id,
                        entity.UserId,
                        entity.Balance,
                        entity.IsDeleted,
                        entity.CreationDate,
                        entity.ExpiredDate,
                        entity.LastModified
                    });   
                    
                    await sqlConnection.ExecuteAsync(createAccountDetailCommand, new
                    {
                        entity.AccountDetail.AccountId,
                        entity.AccountDetail.Description,
                        entity.AccountDetail.Currency,
                        entity.AccountDetail.LimitByOperation,
                        entity.AccountDetail.CreationDate,
                        entity.AccountDetail.LastModified
                    });

                    try
                    {
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        
                        throw new ApplicationApiException(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }

            return await GetAsync(entity.Id, entity.UserId);
        }

        public async Task<Account> UpdateAsync(Account entity)
        {
            var updateAccountCommand = "UPDATE public.\"Accounts\"" +
                "SET" +
                $"\"{nameof(Account.Balance)}\" = @Balance," +
                $"\"{nameof(Account.IsDeleted)}\" = @IsDeleted," +
                $"\"{nameof(Account.LastModified)}\" = @LastModified, " +
                $"\"{nameof(Account.ExpiredDate)}\" = @ExpiredDate" +
                $" WHERE \"{nameof(Account.UserId)}\" = @UserId and \"{nameof(Account.Id)}\" = @Id;";

            var updateAccountDetailCommand = "UPDATE public.\"AccountDetails\"" +
                "SET" +
                $"\"{nameof(AccountDetail.Description)}\" = @Description," +
                $"\"{nameof(AccountDetail.LimitByOperation)}\" = @LimitByOperation," +
                $"\"{nameof(AccountDetail.LastModified)}\" = @LastModified" +
                $" WHERE \"{nameof(AccountDetail.AccountId)}\" = @AccountId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    await sqlConnection.ExecuteAsync(updateAccountCommand, new
                    {
                        entity.Id,
                        entity.UserId,
                        entity.Balance,
                        entity.IsDeleted,
                        entity.ExpiredDate,
                        entity.LastModified
                    });   
                    
                    await sqlConnection.ExecuteAsync(updateAccountDetailCommand, new
                    {
                        entity.AccountDetail.AccountId,
                        entity.AccountDetail.Description,
                        entity.AccountDetail.LimitByOperation,
                        entity.AccountDetail.Currency,
                        entity.AccountDetail.CreationDate,
                        entity.AccountDetail.LastModified
                    });

                    try
                    {
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        
                        throw new ApplicationApiException(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }

            return entity;
        }
        public async Task<Account> GetAsync(Guid accountId, Guid userId)
        {
            var getAccountCommand = @"SELECT * " +
                "FROM public.\"Accounts\" " +
                $"WHERE \"{nameof(Account.Id)}\" = @AccountId AND " +
                $"\"{nameof(Account.UserId)}\" = @UserId and \"{nameof(Account.IsDeleted)}\" = false;";

            var getAccountDetailsCommand = @"SELECT * " +
                 "FROM public.\"AccountDetails\" " +
                 $"WHERE \"{nameof(AccountDetail.AccountId)}\" = @AccountId;";
            
            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                var account =  await sqlConnection.QueryFirstOrDefaultAsync<Account>(getAccountCommand, new
                {
                    AccountId = accountId,
                    UserId = userId
                });
                
                var accountDetail = await sqlConnection.QueryFirstOrDefaultAsync<AccountDetail>(getAccountDetailsCommand, new
                {
                    AccountId = accountId,
                });

                if (account != null)
                {
                    account.AccountDetail = accountDetail;
                }

                return account;
            }
        }
        public async Task<List<Account>> GetListAsync(Guid userId)
        {
            var getAccountsCommand = @"SELECT * " +
                                    "FROM public.\"Accounts\" " +
                                    $"WHERE \"{nameof(Account.UserId)}\" = @UserId and \"{nameof(Account.IsDeleted)}\" = false;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                var account =  await sqlConnection.QueryAsync<Account>(getAccountsCommand, new
                {
                    UserId = userId
                });
                
                return account.ToList();
            }
        }
        
    }
}