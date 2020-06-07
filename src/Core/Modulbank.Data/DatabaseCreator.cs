using System;
using Npgsql;

namespace Modulbank.Data.Context
{
    public class DatabaseCreator<TDatabaseContext> where TDatabaseContext : GeneralContext
    {
        private readonly TDatabaseContext _context;

        public DatabaseCreator(TDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void CreateIfNotExist()
        {
            var databaseExists = CheckDatabaseExists();

            if (databaseExists) return;

            CreateDatabase();
        }

        private bool CheckDatabaseExists()
        {
            using (var connection = new NpgsqlConnection(_context.MaintenanceConnectionString))
            {
                var query = $"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{_context.DatabaseName}'";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();

                    var result = command.ExecuteScalar();

                    connection.Close();

                    return result != null && result.ToString().Equals(_context.DatabaseName);
                }
            }
        }

        private void CreateDatabase()
        {
            using (var connection = new NpgsqlConnection(_context.MaintenanceConnectionString))
            {
                var query = $"CREATE DATABASE \"{_context.DatabaseName}\" WITH OWNER = {_context.DatabaseOwner} ENCODING = 'UTF8' CONNECTION LIMIT = -1;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();

                    var result = command.ExecuteScalar();

                    connection.Close();
                }
            }
        }
    }
}