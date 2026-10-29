using Microsoft.AspNetCore.Mvc;
using ProviderAssignmentStarter.Services;
using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Controllers;

public class ProvidersController(IProviderService service) : Controller
{
    public async Task<IActionResult> Index(string? search, string? status)
    {
        var providers = await service.SearchAsync(search, status);
        return View(new ProviderListViewModel { Providers = providers, Search = search, Status = status });
    }

    public async Task<IActionResult> Details(int id)
    {
        var provider = await service.GetAsync(id);
        return provider is null ? NotFound() : View(provider);
    }

    [HttpGet]
    public IActionResult Create() => View(new ProviderFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProviderFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var id = await service.CreateAsync(model);
        TempData["Success"] = "Provider created successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var provider = await service.GetAsync(id);
        if (provider is null) return NotFound();
        return View(new ProviderFormViewModel { ProviderId = provider.ProviderId, ProviderName = provider.ProviderName, County = provider.County, Status = provider.Status });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProviderFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (!await service.UpdateAsync(model)) return NotFound();
        TempData["Success"] = "Provider updated successfully.";
        return RedirectToAction(nameof(Details), new { id = model.ProviderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await service.SoftDeleteAsync(id)) return NotFound();
        TempData["Success"] = "Provider soft-deleted. The record remains available for audit purposes.";
        return RedirectToAction(nameof(Index));
    }
}
