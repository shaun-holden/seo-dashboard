using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum PaymentStatus { Pending, Paid, Failed, Refunded }

    public class Payment
    {
        public int Id { get; set; }

        public int AthleteId { get; set; }
        public Athlete Athlete { get; set; } = null!;

        [Required]
        public string PayerUserId { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}
