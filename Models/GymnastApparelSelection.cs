using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class GymnastApparelSelection
    {
        public int Id { get; set; }
        public int GymnastId { get; set; }
        public Gymnast Gymnast { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }
}
