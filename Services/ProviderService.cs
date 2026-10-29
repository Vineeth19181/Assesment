using Microsoft.EntityFrameworkCore;
using ProviderAssignmentStarter.Data;
using ProviderAssignmentStarter.Models;
using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Services;

public class ProviderService(ApplicationDbContext db) : IProviderService
{
    public async Task<IReadOnlyList<Provider>> SearchAsync(string? search, string? status)
    {
        var query = db.Providers.AsNoTracking().Include(x => x.Licenses).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x => EF.Functions.Like(x.ProviderName, $"%{term}%") || EF.Functions.Like(x.County, $"%{term}%"));
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status);

        return await query.OrderBy(x => x.ProviderName).ToListAsync();
    }

    public Task<Provider?> GetAsync(int id) =>
        db.Providers.Include(x => x.Licenses).AsNoTracking().SingleOrDefaultAsync(x => x.ProviderId == id);

    public async Task<int> CreateAsync(ProviderFormViewModel model)
    {
        var provider = new Provider
        {
            ProviderName = model.ProviderName.Trim(),
            County = model.County.Trim(),
            Status = model.Status.Trim()
        };
        db.Providers.Add(provider);
        await db.SaveChangesAsync();
        return provider.ProviderId;
    }

    public async Task<bool> UpdateAsync(ProviderFormViewModel model)
    {
        var provider = await db.Providers.SingleOrDefaultAsync(x => x.ProviderId == model.ProviderId);
        if (provider is null) return false;

        provider.ProviderName = model.ProviderName.Trim();
        provider.County = model.County.Trim();
        provider.Status = model.Status.Trim();
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var provider = await db.Providers.SingleOrDefaultAsync(x => x.ProviderId == id);
        if (provider is null) return false;
        db.Remove(provider);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<License?> GetLicenseAsync(int id) =>
        await db.Licenses
            .Include(x => x.Provider)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.LicenseId == id && x.Provider != null);

    public async Task<bool> AddLicenseAsync(LicenseFormViewModel model)
    {
        if (!await db.Providers.AnyAsync(x => x.ProviderId == model.ProviderId)) return false;

        var license = new License
        {
            ProviderId = model.ProviderId,
            LicenseNumber = model.LicenseNumber.Trim(),
            LicenseStatus = model.LicenseStatus.Trim(),
            ExpirationDate = model.ExpirationDate.Date
        };
        db.Licenses.Add(license);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateLicenseAsync(LicenseFormViewModel model)
    {
        var license = await db.Licenses.SingleOrDefaultAsync(x => x.LicenseId == model.LicenseId && x.Provider != null);
        if (license is null) return false;

        license.LicenseNumber = model.LicenseNumber.Trim();
        license.LicenseStatus = model.LicenseStatus.Trim();
        license.ExpirationDate = model.ExpirationDate.Date;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteLicenseAsync(int id)
    {
        var license = await db.Licenses.SingleOrDefaultAsync(x => x.LicenseId == id);
        if (license is null) return false;
        db.Remove(license);
        await db.SaveChangesAsync();
        return true;
    }
}
