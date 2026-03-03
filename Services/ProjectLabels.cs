using GymBudgetApp.Models;

namespace GymBudgetApp.Services
{
    public class ProjectLabels
    {
        public string Meet { get; }
        public string Meets { get; }
        public string Coach { get; }
        public string Coaches { get; }
        public string TeamLevel { get; }
        public string TeamLevels { get; }
        public string AthleteItem { get; }
        public string AthleteItems { get; }

        public bool EnableTeamLevels { get; }
        public bool EnablePerDiem { get; }
        public bool EnableMileage { get; }

        public ProjectLabels(Season? season)
        {
            if (season == null || season.ProjectType == ProjectType.Competition)
            {
                Meet = "Meet";
                Meets = "Meets";
                Coach = "Coach";
                Coaches = "Coaches";
                TeamLevel = "Team Level";
                TeamLevels = "Team Levels";
                AthleteItem = "Athlete Item";
                AthleteItems = "Athlete Items";
                EnableTeamLevels = true;
                EnablePerDiem = true;
                EnableMileage = true;
            }
            else
            {
                Meet = season.MeetLabel;
                Meets = season.MeetLabel + "s";
                Coach = season.CoachLabel;
                Coaches = season.CoachLabel + "s";
                TeamLevel = season.TeamLevelLabel;
                TeamLevels = season.TeamLevelLabel + "s";
                AthleteItem = season.AthleteItemLabel;
                AthleteItems = season.AthleteItemLabel + "s";
                EnableTeamLevels = season.EnableTeamLevels;
                EnablePerDiem = season.EnablePerDiem;
                EnableMileage = season.EnableMileage;
            }
        }

        public static ProjectLabels Default => new ProjectLabels(null);
    }
}
