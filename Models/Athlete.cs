using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Athlete
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Athlete name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string? ParentPhone { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        // Emergency Contact
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [StringLength(50)]
        public string? EmergencyContactPhone { get; set; }

        [StringLength(100)]
        public string? EmergencyContactRelationship { get; set; }

        // Medical Info
        [StringLength(1000)]
        public string? MedicalNotes { get; set; } // Allergies, conditions, medications

        public ICollection<AthleteItemSelection> ItemSelections { get; set; } = new List<AthleteItemSelection>();
    }

    public class AthleteItemSelection
    {
        public int Id { get; set; }
        public int AthleteId { get; set; }
        public Athlete Athlete { get; set; } = null!;
        public int AthleteItemId { get; set; }
        public AthleteItem AthleteItem { get; set; } = null!;
    }
}
