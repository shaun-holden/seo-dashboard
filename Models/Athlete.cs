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
