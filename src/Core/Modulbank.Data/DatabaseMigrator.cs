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

        public void TryUpdateDatabase(Type initialMigrationType)
        {
            var serviceProvider = CreateServices(_context.ConnectionString, initialMigrationType);

            using (var scope = serviceProvider.CreateScope())
            {
                TryUpdateDatabase(scope.ServiceProvider);
            }
        }

        // private

        private IServiceProvider CreateServices(string connectionString, Type initialMigrationType)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(initialMigrationType.Assembly).For.Migrations())
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void TryUpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}