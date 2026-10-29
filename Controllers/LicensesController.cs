using Microsoft.AspNetCore.Mvc;
using ProviderAssignmentStarter.Services;
using ProviderAssignmentStarter.ViewModels;

namespace ProviderAssignmentStarter.Controllers;

public class LicensesController(IProviderService service) : Controller
{
    [HttpGet]
    public IActionResult Create(int providerId) => View(new LicenseFormViewModel { ProviderId = providerId });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LicenseFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (!await service.AddLicenseAsync(model)) return NotFound();
        TempData["Success"] = "License created successfully.";
        return RedirectToAction("Details", "Providers", new { id = model.ProviderId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var license = await service.GetLicenseAsync(id);
        if (license is null) return NotFound();
        return View(new LicenseFormViewModel { LicenseId = license.LicenseId, ProviderId = license.ProviderId, LicenseNumber = license.LicenseNumber, LicenseStatus = license.LicenseStatus, ExpirationDate = license.ExpirationDate });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LicenseFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (!await service.UpdateLicenseAsync(model)) return NotFound();
        TempData["Success"] = "License updated successfully.";
        return RedirectToAction("Details", "Providers", new { id = model.ProviderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int providerId)
    {
        if (!await service.SoftDeleteLicenseAsync(id)) return NotFound();
        TempData["Success"] = "License soft-deleted.";
        return RedirectToAction("Details", "Providers", new { id = providerId });
    }
}
