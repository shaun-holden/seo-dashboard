using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class ParentLink
    {
        public int Id { get; set; }

        [Required]
        public string InviteCode { get; set; } = string.Empty;

        public int AthleteId { get; set; }
        public Athlete Athlete { get; set; } = null!;

        public string? ParentUserId { get; set; }
        public bool IsClaimed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClaimedAt { get; set; }

        // Auto-pay
        public bool AutoPayEnabled { get; set; }
        public string? StripeCustomerId { get; set; }
        public string? StripeSubscriptionId { get; set; }

        // External billing (iClassPro)
        public bool UseExternalBilling { get; set; }

        public static string GenerateCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 8).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }
}
