using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models;

public class PaymentPlanRequest
{
    public int Id { get; set; }

    [Required]
    public string ParentUserId { get; set; } = string.Empty;

    public int AthleteId { get; set; }

    public int RequestedMonths { get; set; }

    public int RequestedStartMonth { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Approved, Denied

    [StringLength(500)]
    public string? AdminResponse { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }
}
