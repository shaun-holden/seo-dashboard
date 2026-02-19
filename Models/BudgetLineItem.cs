namespace GymBudgetApp.Models
{
    public enum BudgetCategory { TeamEntry, SessionCost, Mileage, Flight, Hotel, CarRental, PerDiem, CoachesMisc, EntryFee, OtherMisc }
    public enum EntryFeeType { State, Regional, SpiritFee, EliteCompulsory }
    public class BudgetLineItem
    {
        public int Id { get; set; }
        public BudgetCategory Category { get; set; }
        public EntryFeeType? EntryFeeType { get; set; }
        public bool IsActual { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total => Rate * Quantity;
        public string? Notes { get; set; }
        public int SeasonId { get; set; }
        public int? MeetId { get; set; }
        public int? CoachId { get; set; }
        public Season Season { get; set; } = null!;
        public Meet? Meet { get; set; }
        public Coach? Coach { get; set; }
    }
}