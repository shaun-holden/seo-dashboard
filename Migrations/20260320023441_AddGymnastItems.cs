using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGymnastItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Jacket",
                table: "Gymnasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Leggings",
                table: "Gymnasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Leotard",
                table: "Gymnasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeamBag",
                table: "Gymnasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeamCamp",
                table: "Gymnasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Jacket",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "Leggings",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "Leotard",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "TeamBag",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "TeamCamp",
                table: "Gymnasts");
        }
    }
}
