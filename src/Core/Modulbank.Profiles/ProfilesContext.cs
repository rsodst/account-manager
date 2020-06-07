using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Profiles.Migrations;
using Modulbank.Settings;
using System.Linq;

namespace Modulbank.Profiles
{
    public interface IProfilesContext : IGeneralContext
    {
    }

    public class ProfilesContext : GeneralContext, IProfilesContext
    {
        public ProfilesContext(IOptions<PostgresConnections> connections)
             : base(connections.Value.connectionOptions.Single(p => p.Context == typeof(ProfilesContext).Name))
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankProfileInitial));
        }
    }
}