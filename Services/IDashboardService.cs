using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetAsync();
}
