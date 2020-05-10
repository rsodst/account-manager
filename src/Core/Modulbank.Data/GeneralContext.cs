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

        protected GeneralContext(IOptions<PostgresConnectionOptions> connectionDetails, string databaseName)
        {
            PostgresConnectionString = $"server={connectionDetails.Value.Server};" +
                                       $"userId={connectionDetails.Value.UserId};" +
                                       $"password={connectionDetails.Value.Password};" +
                                       $"Pooling={connectionDetails.Value.Pooling};";

            ConnectionString = PostgresConnectionString + $"Database={databaseName};";

            DatabaseName = databaseName;
        }

        public virtual async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(ConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}