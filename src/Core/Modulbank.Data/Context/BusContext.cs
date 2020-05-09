using Microsoft.Extensions.Options;
using Modulbank.Settings;

namespace Modulbank.Data.Context
{
    public interface IBusContext : IGeneralContext
    {
    }
    
    public class BusContext : GeneralContext, IBusContext
    {
        public BusContext(IOptions<PostgresConnectionOptions> connectionDetails) : base(connectionDetails, "modulbank-bus")
        {
            this.EnsureDatabaseCreation();
        }
    }
}