using Microsoft.Extensions.Options;
using Modulbank.Data;
using Modulbank.Data.Context;
using Modulbank.Settings;
using Modulbank.Users.Migrations;
using System.Linq;

namespace Modulbank.Users
{
    public interface IUsersContext : IGeneralContext
    {
    }

    public class UsersContext : GeneralContext, IUsersContext
    {
        public UsersContext(IOptions<PostgresConnections> connections) 
            : base(connections.Value.connectionOptions.Single(p=>p.Context == typeof(UsersContext).Name))
        {
            this.EnsureDatabaseCreation();
            this.TryApplyMigration(typeof(ModulBankUsersInitial));
        }
    }
}