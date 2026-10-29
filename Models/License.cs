using System.ComponentModel.DataAnnotations;

namespace ProviderAssignmentStarter.Models;

public class License
{
    public int LicenseId { get; set; }
    public int ProviderId { get; set; }

    [Required, StringLength(100)]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string LicenseStatus { get; set; } = "Active";

    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Provider Provider { get; set; } = null!;

    public bool IsExpired => ExpirationDate.Date < DateTime.UtcNow.Date;
}
