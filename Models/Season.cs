namespace GymBudgetApp.Models
{
    public enum ProjectType { Competition, Custom }

    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AthleteCount { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Project type
        public ProjectType ProjectType { get; set; } = ProjectType.Competition;

        // Custom labels (used when ProjectType == Custom)
        public string MeetLabel { get; set; } = "Meet";
        public string CoachLabel { get; set; } = "Coach";
        public string TeamLevelLabel { get; set; } = "Team Level";
        public string AthleteItemLabel { get; set; } = "Athlete Item";

        // Feature toggles (used when ProjectType == Custom)
        public bool EnableTeamLevels { get; set; } = true;
        public bool EnablePerDiem { get; set; } = true;
        public bool EnableMileage { get; set; } = true;

        public ICollection<Meet> Meets { get; set; } = new List<Meet>();
        public ICollection<Coach> Coaches { get; set; } = new List<Coach>();
        public ICollection<TeamLevel> TeamLevels { get; set; } = new List<TeamLevel>();
        public ICollection<SeasonGroup> SeasonGroups { get; set; } = new List<SeasonGroup>();
        public ICollection<SharedFee> SharedFees { get; set; } = new List<SharedFee>();
        public ICollection<SeasonNote> SeasonNotes { get; set; } = new List<SeasonNote>();
    }
}
