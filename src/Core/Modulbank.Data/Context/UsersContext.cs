using Microsoft.Extensions.Options;
using Modulbank.Settings;

namespace Modulbank.Data.Context
{
    public interface IUsersContext : IGeneralContext
    {
    }

    public class UsersContext : GeneralContext, IUsersContext
    {
        public UsersContext(IOptions<PostgresConnectionOptions> connectionDetails) : base(connectionDetails, "modulbank-users")
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration();
        }
    }
}