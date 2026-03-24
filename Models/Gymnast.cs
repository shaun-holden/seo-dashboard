using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Gymnast
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Level { get; set; }

        // Optional items
        public bool Leotard { get; set; }
        public bool Leggings { get; set; }
        public bool TeamBag { get; set; }
        public bool Jacket { get; set; }

        // Required items
        public bool TeamCamp { get; set; } = true;

        // Sizes
        [StringLength(20)]
        public string? ShirtSize { get; set; }

        [StringLength(20)]
        public string? LeotardSize { get; set; }

        [StringLength(20)]
        public string? LeggingsSize { get; set; }

        [StringLength(20)]
        public string? PantsSize { get; set; }

        // Payment plan
        public int? PaymentPlanMonths { get; set; }
        public int? PaymentStartMonth { get; set; } // 1-12

        [StringLength(200)]
        public string? ParentEmail { get; set; }

        [StringLength(200)]
        public string? ParentEmail2 { get; set; }

        // Emergency Contact
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [StringLength(50)]
        public string? EmergencyContactPhone { get; set; }

        [StringLength(100)]
        public string? EmergencyContactRelationship { get; set; }

        [StringLength(1000)]
        public string? MedicalNotes { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
