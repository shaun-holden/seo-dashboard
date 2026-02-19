namespace GymBudgetApp.Models
{
    public enum MealType { Breakfast, Lunch, Dinner }
    public class PerDiemEntry
    {
        public int Id { get; set; }
        public MealType MealType { get; set; }
        public decimal Rate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal Total => Rate * NumberOfDays;
        public bool IsActual { get; set; }
        public int CoachId { get; set; }
        public int MeetId { get; set; }
        public Coach Coach { get; set; } = null!;
        public Meet Meet { get; set; } = null!;
    }
}