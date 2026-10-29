using Microsoft.AspNetCore.Mvc;
using ProviderAssignmentStarter.Services;

namespace ProviderAssignmentStarter.Controllers;

public class DashboardController(IDashboardService service) : Controller
{
    public async Task<IActionResult> Index() => View(await service.GetAsync());
}
