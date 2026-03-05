namespace GymBudgetApp.Models
{
    public enum SharedFeeCategory
    {
        Mileage,
        PerDiem,
        TeamFees,
        Hotel,
        Flights,
        SessionFees,
        Apparel,
        CoachMembership,
        RentalCar,
        MiscExpense,
        BankFees,
        ProcessFees,
        Banquet
    }

    public class SharedFee
    {
        public int Id { get; set; }
        public SharedFeeCategory Category { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncludedInBudget { get; set; } = true;
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public string DisplayName => !string.IsNullOrWhiteSpace(Name) ? Name : FormatCategory(Category);

        private static string FormatCategory(SharedFeeCategory cat) => cat switch
        {
            SharedFeeCategory.PerDiem => "Per Diem",
            SharedFeeCategory.TeamFees => "Team Fees",
            SharedFeeCategory.SessionFees => "Session Fees",
            SharedFeeCategory.CoachMembership => "Coach Membership",
            SharedFeeCategory.RentalCar => "Rental Car",
            SharedFeeCategory.MiscExpense => "Misc Expense",
            SharedFeeCategory.BankFees => "Bank Fees",
            SharedFeeCategory.ProcessFees => "Process Fees",
            _ => cat.ToString()
        };
    }
}
