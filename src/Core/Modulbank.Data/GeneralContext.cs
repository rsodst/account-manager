using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Modulbank.Settings;
using Npgsql;

namespace Modulbank.Data.Context
{
    public interface IGeneralContext
    {
        Task<NpgsqlConnection> CreateConnectionAsync();
    }

    public abstract class GeneralContext : IGeneralContext
    {
        public readonly string ConnectionString;
        public readonly string DatabaseName;
        public readonly string DatabaseOwner = "postgres";
        public readonly string PostgresConnectionString;
        public readonly string MaintenanceConnectionString;

        protected GeneralContext(PostgresConnectionOptions connectionOptions)
        {
            DatabaseName = connectionOptions.DatabaseName;
            DatabaseOwner = connectionOptions.DatabaseOwner;

            PostgresConnectionString = $"server={connectionOptions.Server};" +
                                       $"userId={connectionOptions.UserId};" +
                                       $"password={connectionOptions.Password};" +
                                       $"Pooling={connectionOptions.Pooling};";
                                       
            if (string.IsNullOrEmpty(connectionOptions.SslMode) == false)
            {
                PostgresConnectionString += $"Sslmode={connectionOptions.SslMode};";
            }
                                       
            if (connectionOptions.TrustServerCertificate.HasValue)
            {
                PostgresConnectionString += $"Trust Server Certificate={connectionOptions.TrustServerCertificate};";
            }

            ConnectionString = PostgresConnectionString + $"Database={connectionOptions.DatabaseName};";
            MaintenanceConnectionString = PostgresConnectionString + $"Database={connectionOptions.MaintenanceDatabase};";

        }

        public virtual async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(ConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}