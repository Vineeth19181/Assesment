using ProviderAssignmentStarter.Models;
using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Services;

public interface IProviderService
{
    Task<IReadOnlyList<Provider>> SearchAsync(string? search, string? status);
    Task<Provider?> GetAsync(int id);
    Task<int> CreateAsync(ProviderFormViewModel model);
    Task<bool> UpdateAsync(ProviderFormViewModel model);
    Task<bool> SoftDeleteAsync(int id);
    Task<bool> AddLicenseAsync(LicenseFormViewModel model);
    Task<bool> UpdateLicenseAsync(LicenseFormViewModel model);
    Task<bool> SoftDeleteLicenseAsync(int id);
    Task<License?> GetLicenseAsync(int id);
}
