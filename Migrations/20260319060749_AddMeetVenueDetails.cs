using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetVenueDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Meets",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionSchedule",
                table: "Meets",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EntryCost",
                table: "Meets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ParkingCost",
                table: "Meets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Meets");

            migrationBuilder.DropColumn(
                name: "CompetitionSchedule",
                table: "Meets");

            migrationBuilder.DropColumn(
                name: "EntryCost",
                table: "Meets");

            migrationBuilder.DropColumn(
                name: "ParkingCost",
                table: "Meets");
        }
    }
}
