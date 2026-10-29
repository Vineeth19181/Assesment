using Microsoft.EntityFrameworkCore;
using ProviderAssignmentStarter.Models;

namespace ProviderAssignmentStarter.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Providers.IgnoreQueryFilters().AnyAsync())
            return;

        var providers = new[]
        {
            new Provider { ProviderName = "Fulton Family Services", County = "Fulton", Status = "Active", CreatedDate = DateTime.UtcNow.AddDays(-30) },
            new Provider { ProviderName = "DeKalb Community Care", County = "DeKalb", Status = "Active", CreatedDate = DateTime.UtcNow.AddDays(-25) },
            new Provider { ProviderName = "Peachtree Support Network", County = "Cobb", Status = "Pending", CreatedDate = DateTime.UtcNow.AddDays(-18) },
            new Provider { ProviderName = "Legacy Wellness Partners", County = "Gwinnett", Status = "Inactive", CreatedDate = DateTime.UtcNow.AddDays(-12) },
            new Provider { ProviderName = "Archived Provider Example", County = "Fulton", Status = "Inactive", CreatedDate = DateTime.UtcNow.AddDays(-60), IsDeleted = true, DeletedDate = DateTime.UtcNow.AddDays(-2) }
        };

        db.Providers.AddRange(providers);
        await db.SaveChangesAsync();

        var activeProviders = await db.Providers.OrderBy(x => x.ProviderId).Take(4).ToListAsync();
        db.Licenses.AddRange(
            new License { ProviderId = activeProviders[0].ProviderId, LicenseNumber = "FAM-10001", LicenseStatus = "Active", ExpirationDate = DateTime.UtcNow.Date.AddMonths(8), CreatedDate = DateTime.UtcNow.AddDays(-20) },
            new License { ProviderId = activeProviders[0].ProviderId, LicenseNumber = "FAM-10002", LicenseStatus = "Expired", ExpirationDate = DateTime.UtcNow.Date.AddDays(-20), CreatedDate = DateTime.UtcNow.AddDays(-20) },
            new License { ProviderId = activeProviders[1].ProviderId, LicenseNumber = "COM-20001", LicenseStatus = "Active", ExpirationDate = DateTime.UtcNow.Date.AddMonths(4), CreatedDate = DateTime.UtcNow.AddDays(-18) },
            new License { ProviderId = activeProviders[2].ProviderId, LicenseNumber = "PSN-30001", LicenseStatus = "Suspended", ExpirationDate = DateTime.UtcNow.Date.AddMonths(2), CreatedDate = DateTime.UtcNow.AddDays(-15) },
            new License { ProviderId = activeProviders[3].ProviderId, LicenseNumber = "LWP-40001", LicenseStatus = "Expired", ExpirationDate = DateTime.UtcNow.Date.AddDays(-45), CreatedDate = DateTime.UtcNow.AddDays(-10) },
            new License { ProviderId = activeProviders[4 - 1].ProviderId, LicenseNumber = "LWP-40002", LicenseStatus = "Active", ExpirationDate = DateTime.UtcNow.Date.AddMonths(6), CreatedDate = DateTime.UtcNow.AddDays(-8), IsDeleted = true, DeletedDate = DateTime.UtcNow.AddDays(-1) }
        );
        await db.SaveChangesAsync();
    }
}
