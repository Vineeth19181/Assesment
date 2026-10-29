using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProviderAssignmentStarter.Data;
using ProviderAssignmentStarter.Models;
using Xunit;

namespace ProviderAssignmentStarter.Tests;

public class ApplicationDbContextTests
{
    [Fact]
    public async Task RemoveProvider_IsPersistedAsSoftDelete()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var db = new ApplicationDbContext(options))
        {
            await db.Database.EnsureCreatedAsync();
            db.Providers.Add(new Provider { ProviderName = "Test Provider", County = "Fulton", Status = "Active" });
            await db.SaveChangesAsync();

            var provider = await db.Providers.SingleAsync();
            db.Remove(provider);
            await db.SaveChangesAsync();
        }

        await using (var auditDb = new ApplicationDbContext(options))
        {
            var deleted = await auditDb.Providers.IgnoreQueryFilters().SingleAsync();
            Assert.True(deleted.IsDeleted);
            Assert.NotNull(deleted.DeletedDate);
            Assert.Empty(await auditDb.Providers.ToListAsync());
        }
    }

    [Fact]
    public async Task DeletedProvider_IsExcludedFromStandardProviderQueries()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var db = new ApplicationDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var provider = new Provider { ProviderName = "Deleted Provider", County = "Fulton", Status = "Active" };
        db.Providers.Add(provider);
        await db.SaveChangesAsync();
        db.Remove(provider);
        await db.SaveChangesAsync();

        var visibleProviders = await db.Providers.ToListAsync();
        Assert.Empty(visibleProviders);
    }
}
