using System.ComponentModel.DataAnnotations;

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

        [Range(0, 1000000, ErrorMessage = "Rate must be between 0 and 1,000,000.")]
        public decimal Rate { get; set; }

        [Range(0, 100000, ErrorMessage = "Quantity must be between 0 and 100,000.")]
        public decimal Quantity { get; set; }
        public decimal Total => Rate * Quantity;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
        public int SeasonId { get; set; }
        public int? MeetId { get; set; }
        public int? CoachId { get; set; }
        public int? SeasonGroupId { get; set; }

        // Audit timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public Season Season { get; set; } = null!;
        public Meet? Meet { get; set; }
        public Coach? Coach { get; set; }
        public SeasonGroup? SeasonGroup { get; set; }
    }
}
