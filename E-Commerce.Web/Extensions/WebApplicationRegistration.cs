using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        // It can return Void Also But returning WebApplication is more Fluent API Style
        public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            //if (databaseService.Database.GetAppliedMigrationsAsync().Result.Any()) // Result blocks the thread until the task is completed 
            var pendingMigrations = await databaseService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await databaseService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> MigrateIdentityDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            var pendingMigrations = await databaseService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
               await databaseService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var DataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");
            await DataInitializerService.InitializeAsync();

            return app;
        }

        public static async Task<WebApplication> SeedIdentityDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var DataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
            await DataInitializerService.InitializeAsync();

            return app;
        }
    }
}
