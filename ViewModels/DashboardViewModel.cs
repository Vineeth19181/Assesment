namespace ProviderAssignmentStarter.ViewModels;

public class DashboardViewModel
{
    public int TotalActiveProviders { get; init; }
    public int ActiveProvidersWithActiveLicenses { get; init; }
    public int ActiveProvidersWithExpiredLicenses { get; init; }
    public int ActiveLicenses { get; init; }
    public int ExpiredLicenses { get; init; }
    public IReadOnlyList<StatusCountViewModel> ProvidersByStatus { get; init; } = [];
    public IReadOnlyList<ProviderLicenseSummaryViewModel> ExpiredLicenseProviders { get; init; } = [];
}

public record StatusCountViewModel(string Status, int Count);
public record ProviderLicenseSummaryViewModel(int ProviderId, string ProviderName, string County, int ExpiredLicenseCount);
