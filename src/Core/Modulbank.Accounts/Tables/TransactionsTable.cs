using System;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Modulbank.Accounts;
using Modulbank.Accounts.Domain;
using Modulbank.Data;
using Modulbank.Shared.Exceptions;

namespace Modulbank.Users.Tables
{
    public class TransactionsTable : IGeneralTable<Transaction>
    {
        private readonly IAccountsContext _context;

        public TransactionsTable(IAccountsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<Transaction> CreateAsync(Transaction entity)
        {
            var command = "INSERT INTO public.\"Transactions\" " +
                          "(" +
                          $"\"{nameof(Transaction.Id)}\", " +
                          $"\"{nameof(Transaction.UserId)}\", " +
                          $"\"{nameof(Transaction.WriteOffAccount)}\", " +
                          $"\"{nameof(Transaction.DestinationAccount)}\", " +
                          $"\"{nameof(Transaction.Status)}\", " +
                          $"\"{nameof(Transaction.Type)}\", " +
                          $"\"{nameof(Transaction.CreationDate)}\")" +
                          $"\"{nameof(Transaction.ProceedDate)}\")" +
                          "VALUES (@Id, @UserId, @WriteOffAccount, @DestinationAccount, @Status, @Type, @CreationDate,@ProceedDate);";

            int rowsInserted;

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Id,
                    entity.UserId,
                    entity.WriteOffAccount,
                    entity.DestinationAccount,
                    entity.Status,
                    entity.Type,
                    entity.CreationDate,
                    entity.ProceedDate,
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(Transaction)} object");

            return entity;
        }

        public async Task<Transaction> UpdateAsync(Transaction entity)
        {
           throw new Exception();
        }

        public async Task<Transaction> GetAsync(Guid userId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"Transactions\" " +
                          $"WHERE \"{nameof(Transaction.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Transaction>(command, new {UserId = userId});
            }
        }
    }
}