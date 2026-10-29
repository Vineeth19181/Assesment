using System.ComponentModel.DataAnnotations;

namespace ProviderAssignmentStarter.ViewModels;

public class ProviderFormViewModel
{
    public int ProviderId { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "Provider name")]
    public string ProviderName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string County { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Active";
}
