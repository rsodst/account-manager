using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Profiles.Migrations;
using Modulbank.Settings;

namespace Modulbank.Profiles
{
    public interface IProfilesContext : IGeneralContext
    {
    }

    public class ProfilesContext : GeneralContext, IProfilesContext
    {
        public ProfilesContext(IOptions<PostgresConnectionOptions> connectionDetails) : base(connectionDetails, "modulbank-profiles")
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankProfileInitial));
        }
    }
}