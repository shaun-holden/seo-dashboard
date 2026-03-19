using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string SenderUserId { get; set; } = string.Empty;

        [StringLength(200)]
        public string SenderEmail { get; set; } = string.Empty;

        [Required]
        public string RecipientUserId { get; set; } = string.Empty;

        [StringLength(200)]
        public string RecipientEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        // For group messages - null means direct message
        public int? TeamLevelId { get; set; }

        // Thread support - replies reference the original
        public int? ParentMessageId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
