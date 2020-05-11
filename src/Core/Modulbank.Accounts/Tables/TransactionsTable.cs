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
    public class TransactionsTable 
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
                          $"\"{nameof(Transaction.Currency)}\", " +
                          $"\"{nameof(Transaction.Amount)}\", " +
                          $"\"{nameof(Transaction.CreationDate)}\"," +
                          $"\"{nameof(Transaction.ProceedDate)}\")" +
                          "VALUES (@Id, @UserId, @WriteOffAccount, @DestinationAccount, @Status, @Type, @Currency, @Amount, @CreationDate,@ProceedDate);";

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
                    entity.Currency,
                    entity.Amount
                });
            }

            if (rowsInserted != 1) throw new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to create {nameof(Transaction)} object");

            return entity;
        }

        public async Task<List<Transaction>> GetListAsync(Guid userId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"Transactions\" " +
                          $"WHERE \"{nameof(Transaction.UserId)}\" = @UserId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                var result = await sqlConnection.QueryAsync<Transaction>(command, new {UserId = userId});

                return result.ToList();
            }
        }
        
        public async Task<Transaction> UpdateAsync(Transaction entity)
        {
            var command = "UPDATE public.\"Transactions\" " +
                          "SET" +
                          $"\"{nameof(Transaction.Status)}\" = @Status, " +
                          $"\"{nameof(Transaction.ProceedDate)}\" = @ProceedDate" +
                          $" WHERE \"{nameof(Transaction.Id)}\" = @Id;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Id,
                    entity.ProceedDate,
                    entity.Status
                });
            }

            return entity;
        }
        
        public async Task<Transaction> GetByIdAsync(Guid transactionId)
        {
            var command = @"SELECT * " +
                          "FROM public.\"Transactions\" " +
                          $"WHERE \"{nameof(Transaction.Id)}\" = @TransactionId;";

            using (var sqlConnection = await _context.CreateConnectionAsync())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Transaction>(command, new {TransactionId = transactionId});
            }
        }
    }
}