using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class CommitmentForm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty; // Full form text, HTML allowed

        // JSON array of section titles that require initials, e.g. ["Payment Policy","Attendance Policy","Code of Conduct"]
        public string InitialSections { get; set; } = "[]";

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CommitmentSignature
    {
        public int Id { get; set; }

        public int CommitmentFormId { get; set; }
        public CommitmentForm CommitmentForm { get; set; } = null!;

        public int AthleteId { get; set; }
        public Gymnast Gymnast { get; set; } = null!;

        [Required]
        public string ParentUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ParentEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string SignatureName { get; set; } = string.Empty; // Typed full name

        public DateTime SignedDate { get; set; }

        // JSON object of section -> initials, e.g. {"Payment Policy":"DH","Attendance Policy":"DH"}
        public string Initials { get; set; } = "{}";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
