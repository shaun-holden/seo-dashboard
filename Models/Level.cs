using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Level
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
