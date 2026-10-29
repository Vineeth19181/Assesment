using Microsoft.EntityFrameworkCore;
using ProviderAssignmentStarter.Data;
using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Services;

public class DashboardService(ApplicationDbContext db) : IDashboardService
{
    public async Task<DashboardViewModel> GetAsync()
    {
        var today = DateTime.UtcNow.Date;
        var activeProviders = db.Providers.AsNoTracking();
        var activeLicenses = db.Licenses.AsNoTracking().Where(x => x.Provider != null);

        var expiredProviderRows = await activeProviders
            .Where(p => p.Licenses.Any(l => l.ExpirationDate < today))
            .Select(p => new ProviderLicenseSummaryViewModel(
                p.ProviderId,
                p.ProviderName,
                p.County,
                p.Licenses.Count(l => l.ExpirationDate < today)))
            .OrderByDescending(x => x.ExpiredLicenseCount)
            .ThenBy(x => x.ProviderName)
            .ToListAsync();

        return new DashboardViewModel
        {
            TotalActiveProviders = await activeProviders.CountAsync(),
            ActiveProvidersWithActiveLicenses = await activeProviders.CountAsync(p => p.Licenses.Any(l => l.LicenseStatus == "Active" && l.ExpirationDate >= today)),
            ActiveProvidersWithExpiredLicenses = expiredProviderRows.Count,
            ActiveLicenses = await activeLicenses.CountAsync(l => l.LicenseStatus == "Active" && l.ExpirationDate >= today),
            ExpiredLicenses = await activeLicenses.CountAsync(l => l.ExpirationDate < today),
            ProvidersByStatus = await activeProviders.GroupBy(p => p.Status).Select(g => new StatusCountViewModel(g.Key, g.Count())).OrderBy(x => x.Status).ToListAsync(),
            ExpiredLicenseProviders = expiredProviderRows
        };
    }
}
