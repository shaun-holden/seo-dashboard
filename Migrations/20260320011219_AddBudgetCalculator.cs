using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetCalculator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetCalculatorEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity1 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity2 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity3 = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalculatedTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    TeamLevelId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsPercentage = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCalculatorEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetCalculatorEntries_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCalculatorEntries_SeasonId",
                table: "BudgetCalculatorEntries",
                column: "SeasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCalculatorEntries");
        }
    }
}
