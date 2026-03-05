namespace GymBudgetApp.Models
{
    public class SeasonNote
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
    }
}
