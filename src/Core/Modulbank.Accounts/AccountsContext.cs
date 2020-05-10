using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Profiles.Migrations;
using Modulbank.Settings;

namespace Modulbank.Accounts
{
    public interface IAccountsContext : IGeneralContext
    {
    }

    public class AccountsContext : GeneralContext, IAccountsContext
    {
        public AccountsContext(IOptions<PostgresConnectionOptions> connectionDetails) : base(connectionDetails, "modulbank-accounts")
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankAccountsInitial));
        }
    }
}