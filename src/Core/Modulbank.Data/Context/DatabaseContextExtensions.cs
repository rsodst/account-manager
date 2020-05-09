namespace Modulbank.Data.Context
{
    public static class DatabaseContextExtensions
    {
        public static void EnsureDatabaseCreation<TDatabaseContext>(this TDatabaseContext context)
            where TDatabaseContext : GeneralContext
        {
            var databaseCreator = new DatabaseCreator<TDatabaseContext>(context);

            databaseCreator.Create();
        }
        
        public static void TryApplyMigration<TDatabaseContext>(this TDatabaseContext context)
            where TDatabaseContext : GeneralContext
        {
            var databaseMigrator = new DatabaseMigrator<TDatabaseContext>(context);
            
            databaseMigrator.TryUpdateDatabase();
        }
    }
}