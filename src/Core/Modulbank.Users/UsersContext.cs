using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Settings;
using Modulbank.Users.Migrations;

namespace Modulbank.Users
{
    public interface IUsersContext : IGeneralContext
    {
    }

    public class UsersContext : GeneralContext, IUsersContext
    {
        public UsersContext(IOptions<PostgresConnectionOptions> connectionDetails) : base(connectionDetails, "modulbank-users")
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankUsersInitial));
        }
    }
}