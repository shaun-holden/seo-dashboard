namespace GymBudgetApp.Models
{
    public enum TravelType { Drive, Fly }
    public class Meet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TravelType TravelType { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
        public ICollection<PerDiemEntry> PerDiemEntries { get; set; } = new List<PerDiemEntry>();
    }
}