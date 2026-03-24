namespace GymBudgetApp.Models
{
    public class SeasonGymnast
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public int GymnastId { get; set; }
        public Gymnast Gymnast { get; set; } = null!;
    }
}
