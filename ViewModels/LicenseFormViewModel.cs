using System.ComponentModel.DataAnnotations;

namespace ProviderAssignmentStarter.ViewModels;

public class LicenseFormViewModel
{
    public int LicenseId { get; set; }
    public int ProviderId { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "License number")]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "License status")]
    public string LicenseStatus { get; set; } = "Active";

    [Required, DataType(DataType.Date)]
    [Display(Name = "Expiration date")]
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.Date.AddYears(1);
}
