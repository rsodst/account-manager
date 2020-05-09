using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Modulbank.Data.Context
{
    public class DatabaseMigrator<TDatabaseContext> where
        TDatabaseContext : GeneralContext
    {
        private TDatabaseContext _context;

        public DatabaseMigrator(TDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void TryUpdateDatabase()
        {
            var serviceProvIder = CreateServices(_context.ConnectionString);

            using (var scope = serviceProvIder.CreateScope())
            {
                TryUpdateDatabase(scope.ServiceProvider);
            }
        }

        // private

        private IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(GetType().Assembly).For.Migrations())
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void TryUpdateDatabase(IServiceProvider serviceProvIder)
        {
            var runner = serviceProvIder.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}