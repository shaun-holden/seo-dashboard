using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AthleteItemLabel",
                table: "Seasons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoachLabel",
                table: "Seasons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EnableMileage",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePerDiem",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTeamLevels",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MeetLabel",
                table: "Seasons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProjectType",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeamLevelLabel",
                table: "Seasons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AthleteItemLabel",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "CoachLabel",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "EnableMileage",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "EnablePerDiem",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "EnableTeamLevels",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "MeetLabel",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "ProjectType",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "TeamLevelLabel",
                table: "Seasons");
        }
    }
}
