using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Accounts;
using Modulbank.Accounts.Domain;
using Modulbank.Data;
using Modulbank.Shared.Exceptions;

namespace Modulbank.Users.Tables
{
    public class AccountActionsTable
    {
        private readonly IAccountsContext _context;

        public AccountActionsTable(IAccountsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<AccountAction> CreateAsync(AccountAction entity)
        {
            var command = "INSERT INTO public.\"AccountActions\" " +
                          "(" +
                          $"\"{nameof(AccountAction.Id)}\", " +
                          $"\"{nameof(AccountAction.AccountId)}\", " +
                          $"\"{nameof(AccountAction.Type)}\", " +
                          $"\"{nameof(AccountAction.CreationDate)}\")" +
                          "VALUES (@Id, @AccountId, @Type, @CreationDate);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Id,
                    entity.AccountId,
                    entity.Type,
                    entity.CreationDate,
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(AccountAction)} object");

            return entity;
        }
        
        public async Task<List<AccountAction>> GetListAsync(Guid accountId, Guid userId)
        {
            var command = "SELECT " +
                          "public.\"AccountActions\".\"Id\"," +
                          "public.\"AccountActions\".\"AccountId\"," +
                          "public.\"AccountActions\".\"Type\"," +
                          "public.\"AccountActions\".\"CreationDate\" " +
                          "FROM public.\"AccountActions\" " +
                          "inner join public.\"Accounts\" on public.\"Accounts\".\"Id\" = public.\"AccountActions\".\"AccountId\"" + 
                          $" WHERE \"{nameof(AccountAction.AccountId)}\" = @AccountId and " +
                          $"public.\"Accounts\".\"{nameof(Account.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                var result =  await sqlConnection.QueryAsync<AccountAction>(command, new
                {
                    AccountId = accountId,
                    UserId = userId
                });

                return result.ToList();
            }
        }
    }
}