using Microsoft.AspNetCore.Mvc;
using ProviderAssignmentStarter.Models;

namespace ProviderAssignmentStarter.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Providers");

    public IActionResult Error() => View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
}
