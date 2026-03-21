using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectionDeadlines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SelectionDeadline",
                table: "CompetitionItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SelectionDeadline",
                table: "ApparelItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectionDeadline",
                table: "CompetitionItems");

            migrationBuilder.DropColumn(
                name: "SelectionDeadline",
                table: "ApparelItems");
        }
    }
}
