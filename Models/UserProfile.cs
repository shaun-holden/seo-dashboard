using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models;

public class UserProfile
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Base64-encoded profile picture (JPEG/PNG)</summary>
    public string? ProfilePictureData { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
