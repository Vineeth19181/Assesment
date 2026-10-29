using ProviderAssignmentStarter.Models;

namespace ProviderAssignmentStarter.ViewModels;

public class ProviderListViewModel
{
    public IReadOnlyList<Provider> Providers { get; init; } = [];
    public string? Search { get; init; }
    public string? Status { get; init; }
}
