using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class BudgetCalculatorEntry
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty; // TeamEntry, SessionCost, Mileage, Flights, Hotel, CarRental, PerDiem, Coaches, Miscellaneous, BankFees, Custom

        [StringLength(200)]
        public string? Name { get; set; } // For misc/custom items

        // Input fields stored individually for clarity
        public decimal Amount { get; set; } // Primary amount/rate
        public decimal Quantity1 { get; set; } // e.g. sessions, miles, nights, days
        public decimal Quantity2 { get; set; } // e.g. coaches, rooms, trips
        public decimal Quantity3 { get; set; } // e.g. additional multiplier

        public decimal CalculatedTotal { get; set; } // Auto-calculated

        // For team entry - which level
        public int? TeamLevelId { get; set; }

        // For bank fees - percentage mode
        public bool IsPercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
