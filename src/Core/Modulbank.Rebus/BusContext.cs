using Microsoft.Extensions.Options;
using Modulbank.Settings;
using System.Linq;

namespace Modulbank.Data.Context
{
    public interface IBusContext : IGeneralContext
    {
    }
    
    public class BusContext : GeneralContext, IBusContext
    {
        public BusContext(IOptions<PostgresConnections> connections)
             : base(connections.Value.connectionOptions.Single(p => p.Context == typeof(BusContext).Name))
        {
            this.EnsureDatabaseCreation();
        }
    }
}