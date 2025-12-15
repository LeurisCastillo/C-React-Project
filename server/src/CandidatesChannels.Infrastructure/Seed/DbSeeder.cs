using CandidatesChannels.Application.Contracts.Security;
using CandidatesChannels.Domain.Entities;
using CandidatesChannels.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CandidatesChannels.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            var admin = new User(email: "admin@demo.com", passwordHash: hasher.Hash("Admin123*"), role: "Admin");
            db.Users.Add(admin);
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded default admin user: admin@demo.com / Admin123*");
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product("Mouse Inalámbrico", "Electronics", 19.99m, 50, "Mouse ergonómico 2.4G"),
                new Product("Teclado Mecánico", "Electronics", 59.90m, 25, "Switches azules"),
                new Product("Botella Térmica", "Home", 12.50m, 120, "500ml acero inoxidable"),
                new Product("Camiseta Deportiva", "Sports", 9.99m, 200, "Dry-fit")
            );
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded sample products");
        }
    }
}
