using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class PaymentReminderLog
    {
        public int Id { get; set; }

        public int AthleteId { get; set; }

        [Required]
        public string ParentUserId { get; set; } = string.Empty;

        public int SeasonId { get; set; }

        public int InstallmentNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ReminderType { get; set; } = string.Empty; // "Upcoming", "DueToday", "PastDue"

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
