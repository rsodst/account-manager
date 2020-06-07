using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Profiles.Migrations;
using Modulbank.Settings;
using System.Linq;

namespace Modulbank.Accounts
{
    public interface IAccountsContext : IGeneralContext
    {
    }

    public class AccountsContext : GeneralContext, IAccountsContext
    {
        public AccountsContext(IOptions<PostgresConnections> connections)
             : base(connections.Value.connectionOptions.Single(p => p.Context == typeof(AccountsContext).Name))
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankAccountsInitial));
        }
    }
}