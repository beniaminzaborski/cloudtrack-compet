using CloudTrack.Competitions.Infrastructure.Persistence;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace CloudTrack.Competitions.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MigrateDb(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        CreateDatabaseIfNotExists(serviceProvider);
        RunMigrations(serviceProvider);
        
        return app;
    }

    private static void CreateDatabaseIfNotExists(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        if (!dbCreator.Exists())
        {
            dbCreator.Create();
        }
    }

    private static void RunMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}
