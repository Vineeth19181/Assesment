using System.ComponentModel.DataAnnotations;

namespace ProviderAssignmentStarter.Models;

public class Provider
{
    public int ProviderId { get; set; }

    [Required, StringLength(200)]
    public string ProviderName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string County { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Status { get; set; } = "Active";

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    public ICollection<License> Licenses { get; set; } = new List<License>();
}
